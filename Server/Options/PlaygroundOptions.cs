namespace Playground.Server.Options;

public class PlaygroundOptions
{
    public JwtOptions Jwt { get; set; } = new();
    public DatabaseOptions Database { get; set; } = new();
}
