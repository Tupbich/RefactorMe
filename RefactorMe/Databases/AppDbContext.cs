using Microsoft.EntityFrameworkCore;

namespace RefactorMe.Databases;

/// <summary>
///     Application database EF context.
/// </summary>
/// <remarks>
///     In test project implemented configuration for models.
///     For this task I suppose that there are some models configurations in dependent assemblies.
/// </remarks>
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
    public DbSet<SurveyResult> SurveyResults { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SurveyQuestionAnswerVariant>();
        builder.Entity<BooleanAnswerVariant>().HasBaseType<SurveyQuestionAnswerVariant>();
        builder.Entity<MinimumNumberAnswerVariant>().HasBaseType<SurveyQuestionAnswerVariant>();
        builder.Entity<SelectAnswerVariant>().HasBaseType<SurveyQuestionAnswerVariant>();
        base.OnModelCreating(builder);
    }
}