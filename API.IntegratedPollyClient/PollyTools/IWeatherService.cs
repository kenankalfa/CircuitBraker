using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.IntegratedPollyClient.PollyTools
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast();
    }
}
