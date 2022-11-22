using System.ComponentModel.DataAnnotations;

namespace Budgeteer.Server.Entities;

public class User : BaseEntity
{
    [Required]
    public required string Name { get; set; }
    [Required, EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string HashedPassword { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
}
