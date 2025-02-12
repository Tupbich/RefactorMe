namespace RefactorMe;

/// <summary>
///     Survey model. that users can complete. This type using in database.
/// </summary>
public class Survey
{
    public int Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    ///     Survey is possible to complete
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    ///     Questions of survey
    /// </summary>
    public List<SurveyQuestion> Questions { get; set; }
}