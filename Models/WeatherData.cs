using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherData
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public string Description { get; set; }
        public double Humidity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public string Icon { get; set; }
    }

    public class WeatherForecast
    {
        public string City { get; set; }
        public List<DailyForecast> Daily { get; set; } = new();

        public class DailyForecast
        {
            public DateTime Date { get; set; }
            public double DayTemperature { get; set; }
            public double NightTemperature { get; set; }
            public string Description { get; set; }
            
        }
    }
}
