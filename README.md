
### Изменения:
 - Рефакторинг
 - Небольшие переименования
 - Исправлены ошибки
 - Оптимизировано быстродействие
 - Добавлены огранчиение на длину строк (нужно уточнять требования при проектировании, но всегда можно расширить)
 
### Замечания:
 - Использовалась БД MS SQL Server чтобы быть ближе к используемой БД (уточнил у HR)
 - В реальной БД нужно сохранять ответы на все вопросы, чтобы иметь всю полноту исторических данных, а не только результат
 - В реальном решении - часть тестов должны быть вынесены как Unit тесты в отдельный проект, сейчас всё вместе
 - Можно уйти в хранимые процедуры, и raw SQL, тогда будет не универсальное решение, но можно получить быстродействие получше
 - Комментарии и сообщения на английском языке, можно на русском, я не знаю какие требования в компании

### Дополнительное задание

> Написать sql запрос, выводящий:
>  - имя пользователя
>  - сколько опросов он прошел за 30 дней
>  - сумму баллов получил за эти (30 дней) опросы
>  - сколько всего опросов прошел
>  - сумму баллов получил за эти (все) опросы
---

```sql
    ;with resultsAllCte as (
        select sr.UserId, count(distinct(sr.SurveyId)) as SurveysAllDays, sum(sr.Score) as ScoreSumAllDays
        from SurveyResults sr
        group by sr.UserId
    ),results30daysCte as (
        select sr.UserId, count(distinct(sr.SurveyId)) as Surveys30Days, sum(sr.Score) as ScoreSum30Days
        from SurveyResults sr
        where sr.CreatedAt > DATEADD(day, -30, getutcdate()) -- possible need to cast to begginning of the day by local time
        group by sr.UserId
    )
     select u.[Name]
          , isnull(ScoreSumAllDays, 0) as ScoreSumAllDays
          , isnull(ScoreSum30Days, 0) as ScoreSum30Days
          , isnull(SurveysAllDays, 0) as SurveysAllDays
          , isnull(ScoreSum30Days, 0) as ScoreSum30Days
     from Users u
              left join results30daysCte r30 on u.Id = r30.UserId
              left join resultsAllCte rAll on u.Id = rAll.UserId
```

---

### Результаты оптимизации LinQ to Sql:

### 1. GetSurveys

до:

```sql      
      -- Parameters=[@__userId_0='1'] 
      SELECT [s].[Id], [s1].[Id], [s1].[Text], [s1].[AnswerType]
      FROM [Surveys] AS [s]
      LEFT JOIN [SurveyQuestions] AS [s1] ON [s].[Id] = [s1].[SurveyId]
      WHERE [s].[IsActive] = CAST(1 AS bit) AND EXISTS (
          SELECT 1
          FROM [SurveyResults] AS [s0]
          WHERE [s0].[UserId] = @__userId_0 AND [s0].[SurveyId] = [s].[Id])
      ORDER BY [s].[Id]
```

после:

```sql
      -- Parameters=[@__userId_0='1']
      SELECT [s].[SurveyId], [s].[Id], [s].[Text], [s].[AnswerType]
      FROM [SurveyQuestions] AS [s]
      INNER JOIN [Surveys] AS [s0] ON [s].[SurveyId] = [s0].[Id]
      WHERE [s0].[IsActive] = CAST(1 AS bit) AND EXISTS (
          SELECT 1
          FROM [SurveyResults] AS [s1]
          WHERE [s1].[UserId] = @__userId_0 AND [s1].[SurveyId] = [s].[SurveyId])
```

### 2. SaveAnswersAsync 

до - антипаттерн N+1:
      
```sql
      --Parameters=[@__v_QuestionId_0='1']
      SELECT TOP(1) [s].[Id], [s].[AnswerType], [s].[CreatedOn], [s].[NumberMin], [s].[SurveyId], [s].[Text]
      FROM [SurveyQuestions] AS [s]
      WHERE [s].[Id] = @__v_QuestionId_0
      
      --Parameters=[@__v_QuestionId_0='2']
      SELECT TOP(1) [s].[Id], [s].[AnswerType], [s].[CreatedOn], [s].[NumberMin], [s].[SurveyId], [s].[Text]
      FROM [SurveyQuestions] AS [s]
      WHERE [s].[Id] = @__v_QuestionId_0

      -- Parameters=[@p0='2025-02-16T10:26:46.8030576Z' (DbType = DateTime), @p1='2', @p2='1', @p3='3']
      INSERT INTO [SurveyResults] ([CreatedOn], [Score], [SurveyId], [UserId])
      OUTPUT INSERTED.[Id]
      VALUES (@p0, @p1, @p2, @p3);
```      
   
после:

```sql
      -- Parameters=[@__surveyId_0='1']
      SELECT [s].[Id], [s].[AnswerType], [s].[NumberMin]
      FROM [SurveyQuestions] AS [s]
      WHERE [s].[SurveyId] = @__surveyId_0
      
      -- Parameters=[@p0='2025-02-16T10:49:44.5234943Z' (DbType = DateTime), @p1='2', @p2='1', @p3='3']
      INSERT INTO [SurveyResults] ([CreatedOn], [Score], [SurveyId], [UserId])  
      OUTPUT INSERTED.[Id]
      VALUES (@p0, @p1, @p2, @p3);
```

