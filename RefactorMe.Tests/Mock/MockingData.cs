using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RefactorMe.Databases;

namespace RefactorMe.Tests.Mock;

public static class MockingData
{
    public static IConfiguration CreateAppConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();
    }

    public static IServiceProvider CreateServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_ => CreateAppConfiguration());
        serviceCollection.AddDbContext<TestingAppDbContext>(
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient
        );
        serviceCollection.AddTransient<AppDbContext>(
            provider => provider.GetRequiredService<TestingAppDbContext>()
        );

        serviceCollection.AddTransient<SurveyService>();
        return serviceCollection.BuildServiceProvider();
    }

    public static async Task FillDatabaseWithTestData(this AppDbContext context)
    {
        await context.Users.AddRangeAsync(
            new User { Name = "Алексей" },
            new User { Name = "Пётр" },
            new User { Name = "Екатерина" },
            new User { Name = "Алёна" },
            new User { Name = "Алиса" },
            new User { Name = "Михаил" },
            new User { Name = "Илья" },
            new User { Name = "Александр" }
        );

        await context.SaveChangesAsync();

        await context.Surveys.AddRangeAsync(
            new RefactorMe.Survey
            {
                IsActive = true,
                Name = "Вступительный опрос для мужской вакансии",
                Questions =
                [
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Boolean,
                        Text = "Вы мужчина?"
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Cколько вам лет?",
                        NumberMin = 18
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Сколько можете тягать (кг)?",
                        AnswerVariants =
                        [
                            new MinimumNumberAnswerVariant { IsRightAnswer = true, Value = 100 }
                        ]
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Select,
                        Text = "Предпочитаемый режим работы?",
                        AnswerVariants =
                        [
                            new SelectAnswerVariant
                            {
                                Value = 0,
                                IsRightAnswer = false,
                                Text = "09:00 - 13:00"
                            },
                            new SelectAnswerVariant
                            {
                                IsRightAnswer = true,
                                Value = 1,
                                Text = "09:00 - 18:00"
                            },
                            new SelectAnswerVariant
                            {
                                Value = 2,
                                Text = "13:00 - 14:00",
                                IsRightAnswer = false
                            }
                        ]
                    }
                ]
            },
            new RefactorMe.Survey
            {
                IsActive = true,
                Name = "Вступительный опрос для кадровой вакансии",
                Questions =
                [
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Boolean,
                        Text = "Вы работали раньше HR?"
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Cколько вам лет?",
                        NumberMin = 18
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Сколько соискателей вы в среднем обзванивали в день?",
                        AnswerVariants =
                        [
                            new MinimumNumberAnswerVariant { IsRightAnswer = true, Value = 4 }
                        ]
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Select,
                        Text = "Предпочитаемый режим работы?",
                        AnswerVariants =
                        [
                            new SelectAnswerVariant
                            {
                                Value = 0,
                                IsRightAnswer = false,
                                Text = "09:00 - 13:00"
                            },
                            new SelectAnswerVariant
                            {
                                IsRightAnswer = true,
                                Value = 1,
                                Text = "09:00 - 18:00"
                            },
                            new SelectAnswerVariant
                            {
                                Value = 2,
                                Text = "13:00 - 14:00",
                                IsRightAnswer = false
                            }
                        ]
                    }
                ]
            },
            new RefactorMe.Survey
            {
                IsActive = true,
                Name = "Абсолютно тестовый опрос",
                Questions =
                [
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Boolean,
                        Text = "Тест bool"
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Test obsolute number",
                        NumberMin = 18
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Test number, again",
                        AnswerVariants =
                        [
                            new MinimumNumberAnswerVariant { IsRightAnswer = true, Value = 4 }
                        ]
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Select,
                        Text = "Some variants question",
                        AnswerVariants =
                        [
                            new SelectAnswerVariant
                            {
                                Value = 0,
                                IsRightAnswer = false,
                                Text = "09:00 - 13:00"
                            },
                            new SelectAnswerVariant
                            {
                                IsRightAnswer = true,
                                Value = 1,
                                Text = "09:00 - 18:00"
                            },
                            new SelectAnswerVariant
                            {
                                Value = 2,
                                Text = "13:00 - 14:00",
                                IsRightAnswer = false
                            }
                        ]
                    }
                ]
            },
            new RefactorMe.Survey
            {
                IsActive = false,
                Name = "Абсолютно тестовый опрос (неактивный)",
                Questions =
                [
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Boolean,
                        Text = "Тест bool"
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Test obsolute number",
                        NumberMin = 18
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Number,
                        Text = "Test number, again",
                        AnswerVariants =
                        [
                            new MinimumNumberAnswerVariant { IsRightAnswer = true, Value = 4 }
                        ]
                    },
                    new SurveyQuestion
                    {
                        AnswerType = QuestionAnswerType.Select,
                        Text = "Some variants question",
                        AnswerVariants =
                        [
                            new SelectAnswerVariant
                            {
                                Value = 0,
                                IsRightAnswer = false,
                                Text = "09:00 - 13:00"
                            },
                            new SelectAnswerVariant
                            {
                                IsRightAnswer = true,
                                Value = 1,
                                Text = "09:00 - 18:00"
                            },
                            new SelectAnswerVariant
                            {
                                Value = 2,
                                Text = "13:00 - 14:00",
                                IsRightAnswer = false
                            }
                        ]
                    }
                ]
            }
        );

        await context.SaveChangesAsync();

        await context.SurveyResults.AddRangeAsync(
            new SurveyResult
            {
                CreatedAt = DateTime.Now.AddDays(-31),
                Score = 3,
                SurveyId = 1,
                UserId = 1
            },
            new SurveyResult
            {
                CreatedAt = DateTime.Now.AddDays(-1),
                Score = 2,
                SurveyId = 2,
                UserId = 1
            },
            new SurveyResult
            {
                CreatedAt = DateTime.Now.AddDays(-5),
                Score = 1,
                SurveyId = 3,
                UserId = 1
            },
            new SurveyResult
            {
                Score = 4,
                CreatedAt = DateTime.Now.AddMonths(-1).AddDays(-5),
                SurveyId = 1,
                UserId = 2
            },
            new SurveyResult
            {
                Score = 4,
                CreatedAt = DateTime.Now.AddDays(-10),
                SurveyId = 1,
                UserId = 5
            },
            new SurveyResult
            {
                Score = 4,
                CreatedAt = DateTime.Now.AddMonths(-2),
                SurveyId = 1,
                UserId = 6
            },
            new SurveyResult
            {
                CreatedAt = DateTime.Now.AddDays(-31),
                Score = 1,
                SurveyId = 2,
                UserId = 6
            },
            new SurveyResult
            {
                CreatedAt = DateTime.Now.AddDays(-21),
                Score = 1,
                SurveyId = 4,
                UserId = 6
            }
        );

        await context.SaveChangesAsync();
    }
}