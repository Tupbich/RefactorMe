using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefactorMe;
using RefactorMe.Dal;
using RefactorMe.Dal.Models;

namespace IntegrationTests;

public class CommonFixture: IDisposable
{
    public readonly AppDbContext AppDbContext;
    public readonly ServiceProvider ServiceProvider;
    
    public CommonFixture()
    {
        var connectionString = GetConnectionString();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging());

        serviceCollection.AddScoped<SurveyService>();
        ServiceProvider = serviceCollection.BuildServiceProvider();

        AppDbContext = ServiceProvider.GetRequiredService<AppDbContext>();
        
        AppDbContext.EnsureDatabaseDeleted();
        AppDbContext.EnsureDatabaseCreated();

        SeedDatabase();
    }

    private string GetConnectionString()
    {
        const string connectionStringNane = "TestDbConnection";
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return config.GetConnectionString(connectionStringNane) ?? throw new ArgumentException("ConnectionStringNane not found");
    }
    
    private void SeedDatabase()
    {
        // To avoid conflicts on concurrent - use only for GetSurveysByUserAsync tests
        AppDbContext.Users.Add(new User { Name = "User one" });
        AppDbContext.Users.Add(new User { Name = "User two" });
        
        // To avoid conflicts on concurrent - use only for SaveAnswersAsync tests
        AppDbContext.Users.Add(new User { Name = "User three" });
        AppDbContext.Users.Add(new User { Name = "User four" });
        AppDbContext.Users.Add(new User { Name = "User five" });
        
        AppDbContext.Surveys.Add(new Survey { Name = "Survey one" });
        AppDbContext.Surveys.Add(new Survey { Name = "Survey two" });
        
        AppDbContext.SaveChanges();
        AppDbContext.SurveyQuestions.Add(new SurveyQuestion
        {
            SurveyId = 1,
            Text = "Question one in the survey one. Do you agree?",
            AnswerType = SurveyQuestion.QuestionAnswerType.Boolean,
        });
        AppDbContext.SurveyQuestions.Add(new SurveyQuestion
        {
            SurveyId = 1,
            Text = "Question two in the survey one. Enter the number",
            AnswerType = SurveyQuestion.QuestionAnswerType.Number,
            NumberMin = 50
        });
        AppDbContext.SurveyQuestions.Add(new SurveyQuestion
        {
            SurveyId = 2,
            Text = "Question one in the survey two. Enter the number",
            AnswerType = SurveyQuestion.QuestionAnswerType.Number,
            NumberMin = 100
        });
        AppDbContext.SurveyQuestions.Add(new SurveyQuestion
        {
            SurveyId = 2,
            Text = "Question two in the survey two. Do you agree?",
            AnswerType = SurveyQuestion.QuestionAnswerType.Boolean,
        });
        
        AppDbContext.SaveChanges();
        AppDbContext.SurveyResults.Add(new SurveyResult
        {
            CreatedAt = DateTime.Parse("2025-01-01"),
            Score = 50,
            UserId = 1,
            SurveyId = 1,
        });
        AppDbContext.SurveyResults.Add(new SurveyResult
        {
            CreatedAt = DateTime.Parse("2025-01-02"),
            Score = 51,
            UserId = 1,
            SurveyId = 2,
        });
        AppDbContext.SurveyResults.Add(new SurveyResult
        {
            CreatedAt = DateTime.Parse("2025-01-03"),
            Score = 52,
            UserId = 1,
            SurveyId = 2,
        });

        
        AppDbContext.SurveyResults.Add(new SurveyResult
        {
            CreatedAt = DateTime.Parse("2025-02-01"),
            Score = 60,
            UserId = 2,
            SurveyId = 1,
        });
        
        AppDbContext.SurveyResults.Add(new SurveyResult
        {
            CreatedAt = DateTime.Parse("2025-03-01"),
            Score = 70,
            UserId = 3,
            SurveyId = 1,
        });

        AppDbContext.SaveChanges();
    }

    public void Dispose()
    {
        AppDbContext.EnsureDatabaseDeleted();
        AppDbContext.Dispose();
        ServiceProvider.Dispose();
    }

}