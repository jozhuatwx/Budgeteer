using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Playground.Server.Entities;

[Table("RefreshTokens")]
public class RefreshToken : BaseEntity
{
    [Required]
    public required string Token { get; set; }
    [Required]
    public required DateTime ExpiryDateTime { get; set; }

    [Required]
    public required virtual int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiryDateTime;
}
