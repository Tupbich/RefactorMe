namespace RefactorMe;

/// <summary>
///     Question model of the survey
/// </summary>
/// <remarks>
///     #1: For better refactoring I will suggest a using generic types. It's gets opportunity to create classes with valid
///     type of answer (same to <see cref="SurveyAnswersDto" />)
///     <br />
///     #2: Like #1 but using inherited classes. Set class <see cref="SurveyQuestion" /> and his property
///     <see cref="AnswerType" /> as <see langword="abstract" /> and <c>get-only</c>.
/// </remarks>
public class SurveyQuestion
{
    public int Id { get; set; }
    public int SurveyId { get; set; }

    /// <summary>
    ///     Question's text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Type of answer
    /// </summary>
    public QuestionAnswerType AnswerType { get; set; }

    /// <summary>
    ///     Minimal value for question with type <see cref="QuestionAnswerType.Number"/>. This was replaced with 
    /// </summary>
    [Obsolete]
    public int? NumberMin { get; set; }

    /// <summary>
    ///     All answer variants that's question have
    /// </summary>
    public List<SurveyQuestionAnswerVariant>? AnswerVariants { get; set; }
}