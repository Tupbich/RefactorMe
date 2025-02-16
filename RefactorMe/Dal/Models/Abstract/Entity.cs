using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefactorMe.Dal.Models.Abstract;

public class Entity
{
    // [Editable(false)]
    public int Id { get; set; }
    
    // [Editable(false)]
    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}