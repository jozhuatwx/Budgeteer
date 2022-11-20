using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playground.Server.Entities;

[Table("Users")]
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
