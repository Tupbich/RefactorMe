using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RefactorMe.Databases;
using RefactorMe.Tests.Mock;

namespace RefactorMe.Tests.Survey;

public class SurveyServiceTests
{
    private static readonly object[] SaveSurveyAnswersTestData =
    [
        new object[] { 6, 3, new object[] { false, 10, 2, 2 }, 0 },
        new object[] { 5, 3, new object[] { true, 20, 8, 1 }, 4 }
    ];

    private readonly IServiceProvider _serviceProvider = MockingData.CreateServiceProvider();

    [SetUp]
    public async Task TestsSetup()
    {
        // Init database for tests
        await using var scope = _serviceProvider.CreateAsyncScope();
        await using var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.EnsureCreatedAsync();

        // Add testing data
        await ctx.FillDatabaseWithTestData();
    }

    [OneTimeTearDown]
    public async Task TestsTearDown()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await using var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await ctx.Database.EnsureDeletedAsync();
    }

    [TestCase(1, 0)]
    [TestCase(2, 2)]
    [TestCase(3, 3)]
    [TestCase(5, 2)]
    [TestCase(6, 1)]
    public async Task GetAvailableSurveysTest(int userId, int expectedAvailableSurveys)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<SurveyService>();
        var surveys = await service.GetSurveys(userId);
        Assert.That(surveys, Has.Length.EqualTo(expectedAvailableSurveys));

        foreach (var survey in surveys)
        {
            Assert.That(survey.Id, Is.Not.Default);
            Assert.That(survey.Questions, Is.Not.Null.And.Not.Empty);

            foreach (var question in survey.Questions)
            {
                Assert.That(question.Id, Is.Not.Default);
                Assert.That(question.Text, Is.Not.Default);

                if (question.AnswerVariants is not null)
                    foreach (var variant in question.AnswerVariants)
                        Assert.That(variant.Value, Is.Not.Null);
            }
        }
    }

    [TestCaseSource(nameof(SaveSurveyAnswersTestData))]
    public async Task SaveSurveyAnswersTest(
        int userId,
        int surveyId,
        object[] answers,
        int expectedScore
    )
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<SurveyService>();
        var survey = await scope.ServiceProvider
            .GetRequiredService<AppDbContext>()
            .Surveys
            .Include(s => s.Questions)
            .FirstAsync(s => s.Id == surveyId);

        await service.SaveAnswers(new SurveyAnswersDto
        {
            SurveyId = surveyId,
            UserId = userId,
            Answers = survey.Questions
                .Select((question, i) => new SurveyAnswersDto.SurveyAnswerDto
                {
                    QuestionId = question.Id,
                    Value = answers[i]
                })
                .ToArray()
        });

        var surveyResult = await scope.ServiceProvider
            .GetRequiredService<AppDbContext>()
            .SurveyResults
            .FirstAsync(sr => sr.UserId == userId && sr.SurveyId == surveyId);

        Assert.That(surveyResult.Score, Is.EqualTo(expectedScore));
    }

    [TestCase(1, 3, 2, 6, 3)]
    [TestCase(2, 0, 0, 4, 1)]
    [TestCase(3, 0, 0, 0, 0)]
    [TestCase(5, 4, 1, 4, 1)]
    [TestCase(6, 1, 1, 6, 3)]
    public async Task GetUserInfoTest(
        int userId, 
        int expectedScoreFor30Days,
        int expectedSurveysCountFor30Days,
        int expectedTotalScore,
        int expectedTotalSurveysCount)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<SurveyService>();

        var user = await scope.ServiceProvider
            .GetRequiredService<AppDbContext>()
            .Users
            .FirstAsync(u => u.Id == userId);

        var userInfo = await service.GetUserInfo(userId);

        Assert.Multiple(() =>
        {
            Assert.That(userInfo.UserId, Is.EqualTo(user.Id));
            Assert.That(userInfo.UserName, Is.EqualTo(user.Name));

            Assert.That(userInfo.SurveyScoreInLast30Days, Is.EqualTo(expectedScoreFor30Days));
            Assert.That(userInfo.SurveyPassedInLast30Days, Is.EqualTo(expectedSurveysCountFor30Days));

            Assert.That(userInfo.TotalSurveyScore, Is.EqualTo(expectedTotalScore));
            Assert.That(userInfo.TotalSurveyPassed, Is.EqualTo(expectedTotalSurveysCount));
        });
    }
}