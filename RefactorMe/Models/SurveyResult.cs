namespace RefactorMe;

/// <summary>
///     Result presenting model of user survey completion
/// </summary>
public class SurveyResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SurveyId { get; set; }

    /// <summary>
    ///     When survey result was commited to database
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Total score of answered questions
    /// </summary>
    public int Score { get; set; }
}