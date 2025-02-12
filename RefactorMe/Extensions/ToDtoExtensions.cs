namespace RefactorMe.Extensions;

/// <summary>
///     Extension methods for converting database models to their dto representing models
/// </summary>
public static class ToDtoExtensions
{
    public static SurveyDto.SurveyQuestionAnswerVariantDto ToDto(this SurveyQuestionAnswerVariant source)
    {
        return source switch
        {
            MinimumNumberAnswerVariant v => new SurveyDto.SurveyQuestionAnswerVariantDto { Value = v.Value },
            BooleanAnswerVariant v => new SurveyDto.SurveyQuestionAnswerVariantDto { Value = v.Value },
            SelectAnswerVariant v => new SurveyDto.SurveyQuestionAnswerVariantDto { Text = v.Text, Value = v.Value },
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    public static SurveyDto.SurveyQuestionDto ToDto(this SurveyQuestion source)
    {
        return new SurveyDto.SurveyQuestionDto
        {
            Text = source.Text,
            Id = source.Id,
            Type = source.AnswerType,
            AnswerVariants = source.AnswerVariants?.Select(variant => variant.ToDto()).ToArray()
        };
    }

    public static SurveyDto ToDto(this Survey source)
    {
        return new SurveyDto
        {
            Id = source.Id,
            Questions = source.Questions.Select(question => question.ToDto()).ToArray()
        };
    }
}