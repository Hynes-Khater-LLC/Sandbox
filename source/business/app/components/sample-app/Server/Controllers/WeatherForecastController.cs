using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sample_app.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using sampleApi;
using System.Net.Http;

namespace sample_app.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Shared.WeatherForecast>> GetAsync()
        {
            try
            {


                sampleApi.Client client = new sampleApi.Client("https://localhost:44304", new HttpClient());

                ICollection<sampleApi.WeatherForecast> forecasts = null;

                forecasts = await client.WeatherForecastAsync();


                List<Shared.WeatherForecast> retCollection = new List<Shared.WeatherForecast>();

                foreach(sampleApi.WeatherForecast forecast in forecasts)
                {
                    Shared.WeatherForecast retData = new Shared.WeatherForecast();

                    retData.Date = forecast.Date.DateTime;
                    retData.TemperatureC = forecast.TemperatureC;
                    retData.Summary = forecast.Summary;
                
                    retCollection.Add(retData);
                }


                return retCollection;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
