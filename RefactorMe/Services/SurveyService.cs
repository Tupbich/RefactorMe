using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RefactorMe.Databases;
using RefactorMe.Extensions;

namespace RefactorMe;

/// <summary>
///     Service for getting data
/// </summary>
/// <param name="serviceProvider"></param>
public class SurveyService(IServiceProvider serviceProvider)
{
    /// <summary>
    ///     Receiving from database all active surveys that user not passed
    /// </summary>
    public async Task<SurveyDto[]> GetSurveys(int userId, CancellationToken token = default)
    {
        await using var db = serviceProvider.GetRequiredService<AppDbContext>();

        return await db.Surveys
            .Where(x => x.IsActive && !db.SurveyResults.Any(sr => sr.UserId == userId && sr.SurveyId == x.Id))
            .Include(x => x.Questions)
            .ThenInclude(question => question.AnswerVariants)
            .Select(x => x.ToDto())
            .ToArrayAsync(token);
    }

    /// <summary>
    ///     Saving in database result of survey passing by user
    /// </summary>
    public async Task SaveAnswers(SurveyAnswersDto value)
    {
        await using var db = serviceProvider.GetRequiredService<AppDbContext>();
        await using var tr = await db.Database.BeginTransactionAsync();

        // preload all questions for survey
        await db.SurveyQuestions
            .Where(question => question.SurveyId == value.SurveyId)
            .Include(question => question.AnswerVariants)
            .LoadAsync();

        var surveyResult = new SurveyResult
        {
            UserId = value.UserId,
            SurveyId = value.SurveyId,
            CreatedAt = DateTime.Now,
            Score = 0
        };

        foreach (var answer in value.Answers)
        {
            var question = await db.SurveyQuestions.FindAsync(answer.QuestionId);

            switch (question!.AnswerType)
            {
                // when we got boolean answer
                case QuestionAnswerType.Boolean when (bool)answer.Value:
                    surveyResult.Score++;
                    break;

                //  when we got numeric answer
                case QuestionAnswerType.Number when int.TryParse(answer.Value.ToString(), out var numericAnswer):
                    if (question.AnswerVariants?.Count > 0 &&
                        question.AnswerVariants[0] is MinimumNumberAnswerVariant mnv && numericAnswer > mnv.Value)
                        surveyResult.Score++;
                    // obsolete variant of minimal numeric value
                    else if (numericAnswer > question.NumberMin) surveyResult.Score++;

                    break;

                case QuestionAnswerType.Select when int.TryParse(answer.Value.ToString(), out var numericAnswer):
                    if (question.AnswerVariants
                        ?.OfType<SelectAnswerVariant>()
                        .Any(variant => variant.IsRightAnswer && numericAnswer == variant.Value) == true)
                        surveyResult.Score++;

                    break;
            }
        }

        await db.SurveyResults.AddAsync(surveyResult);
        await db.SaveChangesAsync();
        await tr.CommitAsync();
    }

    /// <summary>
    ///     Get info of user's surveys completion
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<UserSurveyInfoDto> GetUserInfo(int userId, CancellationToken token = default)
    {
        await using var db = serviceProvider.GetRequiredService<AppDbContext>();

        var user = await db.Users.FindAsync([userId], token);
        if (user is null) throw new ArgumentOutOfRangeException(nameof(userId));

        var userSurveys = await db.SurveyResults
            .Where(sr => sr.UserId == user.Id)
            .ToArrayAsync(token);

        var last30DaysAgo = DateTime.Today.AddDays(-30);
        return new UserSurveyInfoDto
        {
            UserId = user.Id,
            UserName = user.Name,
            TotalSurveyPassed = userSurveys.Length,
            TotalSurveyScore = userSurveys.Sum(sr => sr.Score),
            SurveyPassedInLast30Days = userSurveys.Count(sr => sr.CreatedAt >= last30DaysAgo),
            SurveyScoreInLast30Days = userSurveys.Where(sr => sr.CreatedAt >= last30DaysAgo).Sum(sr => sr.Score)
        };

        /* T-SQL script

         -- For best performance
         declare @lastMonth datetime = DATEADD(day, -30, GETDATE())

         select UserId = u.Id,
                UserName = u.Name,
                TotalSurveyPassed = COUNT(sr.Id),
                TotalSurveyScore = SUM(sr.Score),
                SurveyPassedInLast30Days = COUNT(sr_30.Id),
                SurveyScoreInLast30Days = SUM(sr_30.Score)
         from User u
         join SurveyResult sr on sr.UserId == u.Id
         join SurveyResult sr_30 on sr.UserId == u.Id and sr.CreatedAt > @lastMonth
         where u.Id = @userId

         */
    }
}