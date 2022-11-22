using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgeteer.Server.Entities;

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
