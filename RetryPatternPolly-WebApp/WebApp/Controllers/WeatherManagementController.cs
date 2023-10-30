using Microsoft.AspNetCore.Mvc;
using WebApp.Policies;

namespace WebApp.Controllers
{
    public class WeatherManagementController : Controller
    {
        public readonly HttpClient _client;
        private readonly ClientRetryPolicy _retryPolicy;

        public WeatherManagementController(HttpClient client, ClientRetryPolicy retryPolicy)
        {
            _client = client;
            _retryPolicy = retryPolicy;
        }

        [HttpGet]
        [Route("returnWeather/{id}")]
        public async Task<ActionResult> ReturnUser(int id)
        {
            string apiURL = $"https://localhost:44391/api/WeatherForecast/{id}";

            var response = await _retryPolicy.ExponentialHttpRetry.ExecuteAsync(() => _client.GetAsync(apiURL));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success 200");

                return Ok(response);
            }
            else
            {
                Console.WriteLine("Error 500");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
