using System;
using System.Collections.Generic;
using System.IO;  // Добавлено для работы с файлами
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherRepository
    {
        private const string HistoryFile = "weather_history.json";
        private const string SettingsFile = "settings.json";

        public List<WeatherHistoryItem> LoadHistory()
        {
            try
            {
                if (File.Exists(HistoryFile))
                {
                    var json = File.ReadAllText(HistoryFile);
                    return JsonSerializer.Deserialize<List<WeatherHistoryItem>>(json)
                        ?? new List<WeatherHistoryItem>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки истории: {ex.Message}");
            }
            return new List<WeatherHistoryItem>();
        }

        public void SaveHistory(List<WeatherHistoryItem> history)
        {
            try
            {
                var json = JsonSerializer.Serialize(history);
                File.WriteAllText(HistoryFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения истории: {ex.Message}");
            }
        }

        public AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    return JsonSerializer.Deserialize<AppSettings>(json)
                        ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
            }
            return new AppSettings();
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings);
                File.WriteAllText(SettingsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения настроек: {ex.Message}");
            }
        }
    }
}