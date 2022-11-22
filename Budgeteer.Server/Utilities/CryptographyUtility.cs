using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Budgeteer.Server.Utilities;

// Password methods based on https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PasswordHasher.cs
public static class CryptographyUtility
{
    private const int _iterCount = 1000;

    public static Task<string> GenerateRandomCharactersAsync(
        int length, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var rgb = new byte[length];
            var rngCrypt = RandomNumberGenerator.Create();
            rngCrypt.GetBytes(rgb);
            return Convert.ToBase64String(rgb);
        }, cancellationToken);
    }

    public static async Task<string> HashPasswordAsync(
        string password, CancellationToken cancellationToken = default)
    {
        return Convert.ToBase64String(
            await HashPasswordAsync(
                password,
                RandomNumberGenerator.Create(),
                KeyDerivationPrf.HMACSHA512,
                _iterCount,
                128 / 8,
                256 / 8,
                cancellationToken
            ));
    }

    public static async Task<bool> VerifyHashedPasswordAsync(
        string hashedPasswordStr, string password, CancellationToken cancellationToken = default)
    {
        var hashedPassword = Convert.FromBase64String(hashedPasswordStr);

        try
        {
            // Read header information
            Task<uint>[] tasks = {
                ReadNetworkByteOrderAsync(hashedPassword, 1, cancellationToken),
                ReadNetworkByteOrderAsync(hashedPassword, 5, cancellationToken),
                ReadNetworkByteOrderAsync(hashedPassword, 9, cancellationToken)
            };
            var prf = (KeyDerivationPrf)await tasks[0];
            var iterCount = (int)await tasks[1];
            var saltLength = (int)await tasks[2];

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 128 bits
            var subkeyLength = hashedPassword.Length - 13 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }
        catch
        {
            return false;
        }
    }

    private static async Task<byte[]> HashPasswordAsync(
        string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested, CancellationToken cancellationToken = default)
    {
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subkey.Length];
        outputBytes[0] = 0x01; // Format marker
        await Task.WhenAll(
            WriteNetworkByteOrderAsync(outputBytes, 1, (uint)prf, cancellationToken),
            WriteNetworkByteOrderAsync(outputBytes, 5, (uint)iterCount, cancellationToken),
            WriteNetworkByteOrderAsync(outputBytes, 9, (uint)saltSize, cancellationToken)
        );
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);

        return outputBytes;
    }

    private static Task<uint> ReadNetworkByteOrderAsync(
        byte[] buffer, int offset, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
            ((uint)buffer[offset + 0] << 24)
            | ((uint)buffer[offset + 1] << 16)
            | ((uint)buffer[offset + 2] << 8)
            | ((uint)buffer[offset + 3])
        , cancellationToken);
    }

    private static Task WriteNetworkByteOrderAsync(
        byte[] buffer, int offset, uint value, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }, cancellationToken);
    }
}
