namespace RefactorMe;

public class SurveyDto
{
    public required int Id { get; init; }
    public required SurveyQuestionDto[] Questions { get; init; }

    public class SurveyQuestionDto
    {
        public required int Id { get; init; }
        public required QuestionAnswerType Type { get; init; }
        public required string Text { get; init; }

        /// <summary>
        ///     Answer variants to the question, it's always presented only when <see cref="Type" /> equals
        ///     <see cref="QuestionAnswerType.Select" />. In other cases unusable and may be null or empty.
        /// </summary>
        public SurveyQuestionAnswerVariantDto[]? AnswerVariants { get; init; }
    }

    public class SurveyQuestionAnswerVariantDto
    {
        public string? Text { get; init; }
        public required object Value { get; init; }
    }
}