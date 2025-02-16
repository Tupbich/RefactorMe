using RefactorMe.Dal.Models;
using RefactorMe.MsSql.Repositories;

namespace RefactorMe.Swagger.Controllers;

public class SurveyResultController(IBaseRepository<SurveyResult> repository) : BaseController<SurveyResult>(repository);