using System.ComponentModel.DataAnnotations.Schema;

namespace RefactorMe.Dal.Models.Abstract;

public class Entity
{
    public int Id { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}