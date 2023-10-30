using Microsoft.AspNetCore.Mvc;

namespace RetryPatternPolly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            Random random = new Random();

            var failEdge = random.Next(1, 50);

            if (id < failEdge)
            {
                Console.WriteLine("I'm returning Success - 200");
                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray());
            }

            Console.WriteLine("I'm returning Error - 500");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}