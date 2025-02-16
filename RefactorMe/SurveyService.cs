using Microsoft.EntityFrameworkCore;
using RefactorMe.Dal;
using RefactorMe.Dal.Models;
using RefactorMe.Dto;

namespace RefactorMe;

public class SurveyService(AppDbContext db)
{
    public async Task<SurveyDto[]> GetSurveysByUserAsync(int userId)
    {
        return (
                await db
                    .SurveyQuestions
                    .AsNoTracking()
                    .Where(x => x.Survey.IsActive 
                                && db.SurveyResults.Any(sr => sr.UserId == userId && sr.SurveyId == x.SurveyId))
                    .Select(x => new {x.SurveyId, x.Id, x.Text, x.AnswerType})
                    .ToArrayAsync()
            )
            .GroupBy(q => q.SurveyId)
            .Select(g => new SurveyDto
            {
                Id = g.Key,
                Questions = g.Select(q => new SurveyDto.SurveyQuestionDto
                {
                    Id = q.Id,
                    Type = q.AnswerType,
                    Text = q.Text
                }).ToArray() // Convert questions to an array
            })
            .ToArray();
    }
    
    public async Task SaveAnswersAsync(SurveyAnswersDto value)
    {
        var score = await CalculateScoreAsync(value.Answers, value.SurveyId);

        await db.SurveyResults.AddAsync(new SurveyResult
        {
            UserId = value.UserId,
            SurveyId = value.SurveyId,
            Score = score,
        });

        await db.SaveChangesAsync();
    }

    private async Task<int> CalculateScoreAsync(SurveyAnswersDto.SurveyAnswerDto[] answers, int surveyId)
    {
        var questions = await db.SurveyQuestions.AsNoTracking()
            .Where(x => x.SurveyId == surveyId)
            .Select(x => new { x.Id, x.AnswerType, x.NumberMin })
            .ToListAsync();

        if (questions.Count == 0)
        {
            throw new ArgumentException($"Questions for SurveyId: {surveyId} not found");
        }
        
        var score = 0;
        foreach (var a in answers)
        {
            var q = questions.FirstOrDefault(x => x.Id == a.QuestionId);
            if (q == null)
            {
                throw new ArgumentException($"QuestionId: {a.QuestionId} with SurveyId: {surveyId} not found");
            }

            if (q.AnswerType == SurveyQuestion.QuestionAnswerType.Boolean)
            {
                if (bool.TryParse(a.Value.ToString(), out var valueBool))
                {
                    if (valueBool)
                    {
                        score++;
                    }
                }
                else
                {
                    throw new InvalidCastException($"Impossible to cast an answer: {a.Value} to boolean type");
                }
            }
            else if (q.AnswerType == SurveyQuestion.QuestionAnswerType.Number)
            {
                if (int.TryParse(a.Value.ToString(), out var valueInt))
                {
                    if (valueInt > q.NumberMin)
                    {
                        score++;
                    }
                }
                else
                {
                    throw new InvalidCastException($"Impossible to cast an answer: {a.Value} to int type");
                }
            }
        }
        return score;
    }
}