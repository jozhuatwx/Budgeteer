using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaygroundApi.Entities;

[Table("Users")]
public class User : BaseEntity
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Email { get; set; }
}
