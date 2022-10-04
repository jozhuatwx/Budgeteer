using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playground.Application.Entities;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModifiedDateTime { get; set; }
    public DateTime? DeletedDateTime { get; set; }

    public bool IsDeleted => DeletedDateTime != null;
}
