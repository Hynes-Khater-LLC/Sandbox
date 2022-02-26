using System;

namespace sample_api
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public int TemperatureFeelsLikeC { get; set;}

        public int TemperatureFeelsLikeF => 32 + (int)(TemperatureFeelsLikeC / 0.5556);

    }
}
