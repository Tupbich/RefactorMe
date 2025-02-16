using RefactorMe.Dal.Models;
using RefactorMe.MsSql.Repositories;

namespace RefactorMe.Swagger.Controllers;

public class SurveyController(IBaseRepository<Survey> repository) : BaseController<Survey>(repository);