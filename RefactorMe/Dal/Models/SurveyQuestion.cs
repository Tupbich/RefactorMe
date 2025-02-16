using System.ComponentModel.DataAnnotations;
using RefactorMe.Dal.Models.Abstract;

namespace RefactorMe.Dal.Models;

public partial class SurveyQuestion : Entity
{
    public int SurveyId { get; set; }
    
    public QuestionAnswerType AnswerType { get; set; }
    
    [MaxLength(4000)]
    public string Text { get; set; }
    
    public int NumberMin { get; set; }
    
    public Survey Survey { get; set; }
}
