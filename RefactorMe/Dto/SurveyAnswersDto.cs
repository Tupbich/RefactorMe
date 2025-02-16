namespace RefactorMe.Dto;

public record SurveyAnswersDto
{
    public record SurveyAnswerDto
    {
        public int QuestionId { get; init; }
        public object Value { get; init; }
    }

    public int UserId { get; init; }
    public int SurveyId { get; init; }
    public SurveyAnswerDto[] Answers { get; init; }
}