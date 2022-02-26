using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();


            string url = $"https://atlas.microsoft.com/weather/forecast/daily/json?api-version=1.0&query=44.9778%2C93.2650&duration=5&subscription-key=IspvfmeOHOjnFlLoaOie3B8b-eDjACwyG_0SnNHMMjQ";

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

                        double temp1 = forecast.realFeelTemperature.maximum.value;

                        data.TemperatureC = (int)temp;
                      
                        data.Summary = "";

                        data.TemperatureFeelsLikeC = (int)temp1;

                        forecasts.Add(data);
                    }


                }

            }



            return forecasts;


        }
    }
}
