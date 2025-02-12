namespace RefactorMe;

public abstract class SurveyQuestionAnswerVariant
{
    public int Id { get; set; }

    /// <summary>
    ///     Foreign key to <see cref="SurveyQuestion" />.
    /// </summary>
    public int SurveyQuestionId { get; set; }

    /// <summary>
    ///     Current answer variant is good answer for the question
    /// </summary>
    public required bool IsRightAnswer { get; set; }
}

/// <summary>
///     Answer variant for <see cref="QuestionAnswerType.Boolean" />
/// </summary>
public class BooleanAnswerVariant : SurveyQuestionAnswerVariant
{
    public required bool Value { get; set; }
}

/// <summary>
///     Answer variant for <see cref="QuestionAnswerType.Number" />
/// </summary>
public class MinimumNumberAnswerVariant : SurveyQuestionAnswerVariant
{
    public required int Value { get; set; }
}

/// <summary>
///     Answer variant for <see cref="QuestionAnswerType.Select" />.
/// </summary>
/// <remarks>
///     This class can be merged with <see cref="MinimumNumberAnswerVariant" />, but I don't see practical advantage of
///     this decision.
/// </remarks>
public class SelectAnswerVariant : SurveyQuestionAnswerVariant
{
    public required string Text { get; set; }
    public required int Value { get; set; }
}