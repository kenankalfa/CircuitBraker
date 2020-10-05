using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.IntegratedPollyClient.PollyTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.IntegratedPollyClient.Controllers
{
    [ApiController]
    [Route("api/havadurumclient")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _service;

        public WeatherForecastController(IWeatherService service)
        {
            _service = service;
        }

        [HttpGet("getir")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            Console.WriteLine("Fetching weather from API");

            try
            {

                var forecast = await _service.GetWeatherForecast();
                if (forecast != null)
                {
                    Console.WriteLine("Weather successfully fetched");
                    return Ok(forecast);
                }
                else
                {
                    Console.WriteLine("Failed to fetch weather");
                    return NotFound("Failed to fetch weather");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get weather");
                return StatusCode(500, new { Error = "Something happened" });
            }
        }
    }
}
