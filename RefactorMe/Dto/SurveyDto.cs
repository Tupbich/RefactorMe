using RefactorMe.Dal.Models;

namespace RefactorMe.Dto;

public record SurveyDto
{
    public record SurveyQuestionDto
    {
        public int Id { get; init; }
        public SurveyQuestion.QuestionAnswerType Type { get; init; }
        public string Text { get; init; }
    }

    public int Id { get; init; }
    public SurveyQuestionDto[] Questions { get; init; }
}