using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefactorMe.Databases;

namespace RefactorMe.Tests.Mock;

public class TestingAppDbContext : AppDbContext
{
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new SurveyDbConfiguration());
        builder.ApplyConfiguration(new SurveyQuestionDbConfiguration());
        builder.ApplyConfiguration(new SurveyResultDbConfiguration());
        builder.ApplyConfiguration(new UserDbConfiguration());
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("UnitTests");
        optionsBuilder.ConfigureWarnings(builder => { builder.Ignore(InMemoryEventId.TransactionIgnoredWarning); });
        base.OnConfiguring(optionsBuilder);
    }
}

public class SurveyDbConfiguration : IEntityTypeConfiguration<RefactorMe.Survey>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RefactorMe.Survey> builder)
    {
        builder.HasKey(survey => survey.Id);
        builder.Property(survey => survey.Name).HasMaxLength(4_000);
        builder.HasMany(survey => survey.Questions)
            .WithOne()
            .HasForeignKey(question => question.SurveyId);
    }
}

public class SurveyQuestionDbConfiguration : IEntityTypeConfiguration<SurveyQuestion>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.HasKey(question => question.Id);
        builder.HasOne<RefactorMe.Survey>()
            .WithMany(survey => survey.Questions)
            .HasForeignKey(question => question.SurveyId);

        builder.HasMany(question => question.AnswerVariants)
            .WithOne()
            .HasForeignKey(variant => variant.SurveyQuestionId);
    }
}

public class SurveyQuestionAnswerVariantDbConfiguration : IEntityTypeConfiguration<SurveyQuestionAnswerVariant>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SurveyQuestionAnswerVariant> builder)
    {
        builder.HasKey(variant => variant.Id);
        builder.HasOne<SurveyQuestion>()
            .WithMany(question => question.AnswerVariants)
            .HasForeignKey(variant => variant.SurveyQuestionId);
    }
}

public class SurveyResultDbConfiguration : IEntityTypeConfiguration<SurveyResult>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SurveyResult> builder)
    {
        builder.HasKey(result => result.Id);
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(result => result.UserId);

        builder.HasOne<RefactorMe.Survey>()
            .WithMany()
            .HasForeignKey(result => result.SurveyId);
    }
}

public class UserDbConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Name).HasMaxLength(4_000);
    }
}