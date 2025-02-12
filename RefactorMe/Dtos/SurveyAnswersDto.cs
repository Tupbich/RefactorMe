namespace RefactorMe;

/// <summary>
///     User's completion of the survey
/// </summary>
public class SurveyAnswersDto
{
    public required int UserId { get; init; }
    public required int SurveyId { get; init; }

    /// <summary>
    ///     Answers to questions
    /// </summary>
    public required SurveyAnswerDto[] Answers { get; init; }

    /// <summary>
    ///     User's answer to the question
    /// </summary>
    public class SurveyAnswerDto
    {
        public required int QuestionId { get; set; }
        public required object Value { get; set; }
    }
}