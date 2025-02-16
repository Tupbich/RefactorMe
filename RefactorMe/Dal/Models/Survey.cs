using System.ComponentModel.DataAnnotations;
using RefactorMe.Dal.Models.Abstract;

namespace RefactorMe.Dal.Models;

public class Survey : Entity
{
    [MaxLength(4000)]
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<SurveyQuestion> Questions { get; set; } 
}