using RefactorMe.Dal.Models;
using RefactorMe.MsSql.Repositories;

namespace RefactorMe.Swagger.Controllers;

public class UsersController(IBaseRepository<User> repository) : BaseController<User>(repository);