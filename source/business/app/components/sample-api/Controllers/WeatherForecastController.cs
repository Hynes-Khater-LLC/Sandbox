using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Darnton.Blazor.DeviceInterop.Geolocation;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace sample_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("{cord}")]
        public async Task<IEnumerable<WeatherForecast>> GetAsync(string cord)
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            // 30.266666,-97.733330
            //string url = $"https://atlas.microsoft.com/weather/forecast/daily/json?api-version=1.0&query=29.76328%2C-95.36327&duration=5&subscription-key=IspvfmeOHOjnFlLoaOie3B8b-eDjACwyG_0SnNHMMjQ";
            string url = $"https://atlas.microsoft.com/weather/forecast/daily/json?api-version=1.0&query=" +cord+ "&duration=5&subscription-key=IspvfmeOHOjnFlLoaOie3B8b-eDjACwyG_0SnNHMMjQ";
            var client = new RestClient(url);

            var request = new RestRequest(Method.GET);

            request.AddHeader("cache-control", "no-cache");

            request.AddHeader("x-ms-client-id", "be56900c-69ed-4902-9f56-99af97bafdde");

            IRestResponse response = client.Execute(request);

            List<WeatherForecast> forecasts = new List<WeatherForecast>();

            if (response.IsSuccessful)
            {
                string content = response.Content;

                Root weatherObjects = JsonConvert.DeserializeObject<Root>(content);

                List<Forecast> mapForecasts = weatherObjects.forecasts;

                if (mapForecasts != null)
                {
                    foreach (Forecast forecast in mapForecasts)
                    {
                        WeatherForecast data = new WeatherForecast();

                        data.Date = forecast.date;

                        double temp = forecast.temperature.maximum.value;

                        data.TemperatureC = (int)temp;
                        data.Summary = forecast.day.longPhrase;

                        forecasts.Add(data);
                    }


                }

            }



            return forecasts;


        }
    }
}
