using RefactorMe.Dal.Models.Abstract;

namespace RefactorMe.Dal.Models;

public class SurveyResult : Entity
{
    public int UserId { get; set; }
    public int SurveyId { get; set; }
    public int Score { get; set; }
}