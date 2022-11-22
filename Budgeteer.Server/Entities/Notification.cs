using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgeteer.Server.Entities;

public class Notification : BaseEntity
{
    [Required]
    public required string Message { get; set; }
    public DateTime? ReadDateTime { get; set; }

    [Required]
    public required virtual int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    public bool IsRead => ReadDateTime != null;
}
