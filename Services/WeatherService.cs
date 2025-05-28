using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "bcdbb8a66883d4454488a21cd71d7d58";
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/";

        public WeatherService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<WeatherData> GetCurrentWeatherAsync(string city)
        {
            var response = await _httpClient.GetAsync(
                $"weather?q={city}&appid={ApiKey}&units=metric&lang=ru");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<OpenWeatherCurrentResponse>(json);

            return new WeatherData
            {
                City = data.Name,
                Temperature = data.Main.Temp,
                Description = data.Weather[0].Description,
                Icon = $"https://openweathermap.org/img/wn/{data.Weather[0].Icon}@2x.png"
            };
        }

        public async Task<FiveDayForecast> Get5DayForecastAsync(string city)
        {
            var response = await _httpClient.GetAsync(
                $"forecast?q={city}&appid={ApiKey}&units=metric&lang=ru&cnt=40");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(json);

            return new FiveDayForecast
            {
                City = data.City.Name,
                Daily = data.List
                    .Where(x => x.DateTime.TimeOfDay == TimeSpan.FromHours(12))
                    .Take(5)
                    .Select(item => new DailyForecast
                    {
                        Date = item.DateTime,
                        DayTemperature = item.Main.Temp,
                        Description = item.Weather[0].Description,
                        Icon = $"https://openweathermap.org/img/wn/{item.Weather[0].Icon}@2x.png"
                    }).ToList()
            };
        }

        // Вспомогательные классы для десериализации JSON
        private class OpenWeatherCurrentResponse
        {
            public string Name { get; set; }
            public MainData Main { get; set; }
            public WeatherInfo[] Weather { get; set; }
        }

        private class OpenWeatherForecastResponse
        {
            public CityInfo City { get; set; }
            public List<ForecastItem> List { get; set; }
        }

        private class MainData
        {
            public double Temp { get; set; }
        }

        private class WeatherInfo
        {
            public string Description { get; set; }
            public string Icon { get; set; }
        }

        private class CityInfo
        {
            public string Name { get; set; }
        }

        private class ForecastItem
        {
            [JsonProperty("dt")]
            public long UnixTime { get; set; }
            public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(UnixTime).DateTime;
            public MainData Main { get; set; }
            public List<WeatherInfo> Weather { get; set; }
        }
    }
}