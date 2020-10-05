using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.IntegratedPollyClient.PollyTools
{
    public class HttpWeatherService : IWeatherService
    {
        private readonly HttpClient _client;

        public HttpWeatherService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            var response = await _client.GetAsync("/api/havadurum/getir");

            if (response.IsSuccessStatusCode)
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(stringContent);
            }
            else
            {
                return null;
            }
        }
    }
}
