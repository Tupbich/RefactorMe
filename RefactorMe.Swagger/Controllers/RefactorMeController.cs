// using Microsoft.AspNetCore.Mvc;
// using RefactorMe.MsSql.Dal;
//
// namespace RefactorMe.Swagger.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class RefactorMeController : ControllerBase
// {
//     private readonly ILogger<RefactorMeController> _logger;
//     private MsSqlDbContext _dbContext;
//
//     public RefactorMeController(ILogger<RefactorMeController> logger)
//     {
//         _logger = logger;
//     }
//
//     // [HttpGet(Name = "GetWeatherForecast")]
//     // public IEnumerable<WeatherForecast> Get()
//     // {
//     //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//     //         {
//     //             Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//     //             TemperatureC = Random.Shared.Next(-20, 55),
//     //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//     //         })
//     //         .ToArray();
//     // }
//     
//     [HttpGet(Name = "AddUser")]
//     public IEnumerable<WeatherForecast> GetUser()
//     {
//         return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//             {
//                 Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                 TemperatureC = Random.Shared.Next(-20, 55),
//                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//             })
//             .ToArray();
//     }
//     
//     [HttpGet(Name = "AddUser")]
//     public IEnumerable<WeatherForecast> ()
//     {
//         return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//             {
//                 Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                 TemperatureC = Random.Shared.Next(-20, 55),
//                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//             })
//             .ToArray();
//     }
// }