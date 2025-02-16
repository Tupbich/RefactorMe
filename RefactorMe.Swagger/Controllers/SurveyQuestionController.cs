using RefactorMe.Dal.Models;
using RefactorMe.MsSql.Repositories;

namespace RefactorMe.Swagger.Controllers;

public class SurveyQuestionController(IBaseRepository<SurveyQuestion> repository) : BaseController<SurveyQuestion>(repository);