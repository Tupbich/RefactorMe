using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RefactorMe;
using RefactorMe.Dal.Models;
using RefactorMe.Dto;

namespace IntegrationTests;

public class SurveyServiceTests(CommonFixture fixture) : IClassFixture<CommonFixture>
{
    private IServiceScope GetSurveyService(out SurveyService surveyService)
    {
        var scope = fixture.ServiceProvider.CreateScope();
        surveyService = scope.ServiceProvider.GetRequiredService<SurveyService>();
        return scope;
    }
    
#region GetSurveysByUserAsync

    [Fact]
    public async Task GetSurveysByUserAsync_GetForUser1_ReturnsTwoSurveysAndIgnoreDuplicates()
    {
        using var scope = GetSurveyService(out var surveyService);
        
        var surveys = await surveyService.GetSurveysByUserAsync(userId: 1);

        var expectedSurveys = new[]
        {
            new SurveyDto
            { 
                Id = 1, 
                Questions = 
                [
                    new SurveyDto.SurveyQuestionDto { Id = 1, Type = SurveyQuestion.QuestionAnswerType.Boolean, Text = "Question one in the survey one. Do you agree?" },
                    new SurveyDto.SurveyQuestionDto { Id = 2, Type = SurveyQuestion.QuestionAnswerType.Number, Text = "Question two in the survey one. Enter the number" }
                ]
            },
            new SurveyDto
            {
                Id = 2,
                Questions = 
                [
                    new SurveyDto.SurveyQuestionDto { Id = 3, Type = SurveyQuestion.QuestionAnswerType.Number, Text = "Question one in the survey two. Enter the number"},
                    new SurveyDto.SurveyQuestionDto { Id = 4, Type = SurveyQuestion.QuestionAnswerType.Boolean, Text = "Question two in the survey two. Do you agree?" }
                ]
            }
        };
        
        Assert.Equivalent(expectedSurveys, surveys);
    }
    
    [Fact]
    public async Task GetSurveysByUserAsync_GetForUser2_ReturnsOneSurvey()
    {
        using var scope = GetSurveyService(out var surveyService);
        
        var surveys = await surveyService.GetSurveysByUserAsync(userId: 2);
        
        Assert.Single(surveys);
    }
    
    [Fact]
    public async Task GetSurveysByUserAsync_GetForUser100_NoResults()
    {
        using var scope = GetSurveyService(out var surveyService);
        
        var surveys = await surveyService.GetSurveysByUserAsync(userId: 100);
        
        Assert.Empty(surveys);
    }

#endregion
    
#region SaveAnswersAsync

    [Fact]
    public async Task SaveAnswersAsync_SaveForUser3_AnswerAddedAndScoreEquals2()
    {
        var userId = 3;
        
        using var scope = GetSurveyService(out var surveyService);
        
        var answersCountBefore = await fixture.AppDbContext.SurveyResults.CountAsync(x => x.UserId == userId);
        Assert.Equal(1, answersCountBefore);
        
        var answers = new SurveyAnswersDto
        {
            UserId = userId,
            SurveyId = 1,
            Answers =
            [
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 1, Value = true },
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 2, Value = 70 }
            ]
        };
        await surveyService.SaveAnswersAsync(answers);
        
        var answersCountAfter = await fixture.AppDbContext.SurveyResults.CountAsync(x => x.UserId == userId);
        Assert.Equal(2, answersCountAfter);
        var lastAnswer = await fixture.AppDbContext.SurveyResults.OrderByDescending(x => x.Id)
            .FirstAsync(x => x.UserId == userId);
        Assert.Equal(2, lastAnswer.Score);
    }
    
    [Fact]
    public async Task SaveAnswersAsync_SaveForUser4_AnswerAddedAndScoreEquals0()
    {
        var userId = 4;
        
        using var scope = GetSurveyService(out var surveyService);
        
        var answersCountBefore = await fixture.AppDbContext.SurveyResults.CountAsync(x => x.UserId == userId);
        Assert.Equal(0, answersCountBefore);
        
        var answers = new SurveyAnswersDto
        {
            UserId = userId,
            SurveyId = 1,
            Answers =
            [
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 1, Value = false },
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 2, Value = 10 }
            ]
        };
        await surveyService.SaveAnswersAsync(answers);
        
        var answersCountAfter = await fixture.AppDbContext.SurveyResults.CountAsync(x => x.UserId == userId);
        Assert.Equal(1, answersCountAfter);
        var lastAnswer = await fixture.AppDbContext.SurveyResults.OrderByDescending(x => x.Id)
            .FirstAsync(x => x.UserId == userId);
        Assert.Equal(0, lastAnswer.Score);
    }
    
    [Fact]
    public async Task SaveAnswersAsync_SaveForUser5_AnswerAddedAndScoreEquals1()
    {
        var userId = 5;
        
        using var scope = GetSurveyService(out var surveyService);
        
        var answers = new SurveyAnswersDto
        {
            UserId = userId,
            SurveyId = 2,
            Answers =
            [
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 3, Value = 101 },
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 4, Value = false }
            ]
        };
        await surveyService.SaveAnswersAsync(answers);
        
        var answersCountAfter = await fixture.AppDbContext.SurveyResults.CountAsync(x => x.UserId == userId);
        Assert.Equal(1, answersCountAfter);
        var lastAnswer = await fixture.AppDbContext.SurveyResults.OrderByDescending(x => x.Id)
            .FirstAsync(x => x.UserId == userId);
        Assert.Equal(1, lastAnswer.Score);
    }
    
    [Fact]
    public async Task SaveAnswersAsync_IncorrectSurveyId_ThrowExceptionQuestionsNotFound()
    {
        using var scope = GetSurveyService(out var surveyService);

        var surveyId = 100;
        var answers = new SurveyAnswersDto
        {
            UserId = 5,
            SurveyId = surveyId,
            Answers = []
        };
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => 
            await surveyService.SaveAnswersAsync(answers)
        );
        
        Assert.Equal($"Questions for SurveyId: {surveyId} not found", exception.Message);
    }
    
    [Fact]
    public async Task SaveAnswersAsync_IncorrectQuestionId_ThrowExceptionQuestionNotFound()
    {
        using var scope = GetSurveyService(out var surveyService);

        var surveyId = 1;
        var questionId = 100;
        var answers = new SurveyAnswersDto
        {
            UserId = 5,
            SurveyId = surveyId,
            Answers =
            [
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 100, Value = false }
            ]
        };
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => 
            await surveyService.SaveAnswersAsync(answers)
        );
        
        Assert.Equal($"QuestionId: {questionId} with SurveyId: {surveyId} not found", exception.Message);
    }
    
    [Fact]
    public async Task SaveAnswersAsync_IncorrectAnswerType_ThrowExceptionImpossibleToCast()
    {
        using var scope = GetSurveyService(out var surveyService);

        var incorrectBoolean = "incorrectBoolean";
        var answers = new SurveyAnswersDto
        {
            UserId = 5,
            SurveyId = 1,
            Answers =
            [
                new SurveyAnswersDto.SurveyAnswerDto { QuestionId = 1, Value = incorrectBoolean}
            ]
        };
        var exception = await Assert.ThrowsAsync<InvalidCastException>(async () => 
            await surveyService.SaveAnswersAsync(answers)
        );
        
        Assert.Equal($"Impossible to cast an answer: {incorrectBoolean} to boolean type", exception.Message);
    }
    
#endregion
}