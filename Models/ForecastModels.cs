using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    // Модель для текущей погоды
   

    // Модель для прогноза на 5 дней
    public class FiveDayForecast
    {
        public string City { get; set; }
        public List<DailyForecast> Daily { get; set; }
    }

    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public double DayTemperature { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    // Модель для настроек
    public class AppSettings
    {
        public List<string> FavoriteCities { get; set; } = new List<string>();
    }
}
