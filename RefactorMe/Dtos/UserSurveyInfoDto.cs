namespace RefactorMe;

/// <summary>
///     Data of user's survey completion
/// </summary>
public class UserSurveyInfoDto
{
    public required int UserId { get; init; }
    public required string UserName { get; init; }

    /// <summary>
    ///     Survey complete count in last 30 days
    /// </summary>
    public required int SurveyPassedInLast30Days { get; init; }

    /// <summary>
    ///     Score of completed surveys in last 30 days
    /// </summary>
    public required int SurveyScoreInLast30Days { get; init; }

    /// <summary>
    ///     Total surveys complete (all-time)
    /// </summary>
    public required int TotalSurveyPassed { get; init; }

    /// <summary>
    ///     Total survey score (all-time)
    /// </summary>
    public required int TotalSurveyScore { get; init; }
}