using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;


namespace WeatherApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly WeatherService _weatherService;
        private readonly WeatherRepository _repository;

        [ObservableProperty]
        private string _searchQuery = "Москва";

        [ObservableProperty]
        private WeatherData? _currentWeather;

        [ObservableProperty]
        private FiveDayForecast? _forecast;

        [ObservableProperty]
        private ObservableCollection<WeatherHistoryItem> _weatherHistory;

        [ObservableProperty]
        private List<string> _favoriteCities;

        [ObservableProperty]
        private bool _isFavorite;

        public MainViewModel()
        {
            _weatherService = new WeatherService();
            _repository = new WeatherRepository();

            WeatherHistory = new ObservableCollection<WeatherHistoryItem>(_repository.LoadHistory());
            FavoriteCities = _repository.LoadSettings().FavoriteCities;
        }

        [RelayCommand]
        private async Task LoadWeather(string? city = null)
        {
            var targetCity = city ?? SearchQuery;
            try
            {
                CurrentWeather = await _weatherService.GetCurrentWeatherAsync(targetCity);
                Forecast = await _weatherService.Get5DayForecastAsync(targetCity);

                if (WeatherHistory.Count == 0)
                {
                    WeatherHistory.Add(new WeatherHistoryItem
                    {
                        City = targetCity,
                        Temperature = CurrentWeather.Temperature,
                        RequestTime = DateTime.Now
                    });
                    CollectionViewSource.GetDefaultView(WeatherHistory).Refresh();
                }
                else if (WeatherHistory[0].City == targetCity)
                {

                    WeatherHistory.Insert(0, new WeatherHistoryItem
                    {
                        City = targetCity,
                        Temperature = CurrentWeather.Temperature,
                        RequestTime = DateTime.Now
                    });
                }
                else
                {
                    WeatherHistory.Add(new WeatherHistoryItem
                    {
                        City = targetCity,
                        Temperature = CurrentWeather.Temperature,
                        RequestTime = DateTime.Now
                    });
                    CollectionViewSource.GetDefaultView(WeatherHistory).Refresh();
                }

                if (WeatherHistory.Count > 0 && WeatherHistory[WeatherHistory.Count - 1].City == targetCity)
                {
                    // не закрывать приложение, если достигли конца списка историй запросов
                    Application.Current.MainWindow.Activate();
                    return;
                }
                _repository.SaveHistory(new List<WeatherHistoryItem>(WeatherHistory));

                IsFavorite = FavoriteCities.Contains(targetCity);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ToggleFavorite()
        {
            if (CurrentWeather == null) return;

            var city = CurrentWeather.City;
            if (IsFavorite)
            {
                FavoriteCities.Remove(city);
            }
            else if (!FavoriteCities.Contains(city))
            {
                FavoriteCities.Add(city);
            }

            IsFavorite = !IsFavorite;
            _repository.SaveSettings(new AppSettings { FavoriteCities = FavoriteCities });
        }
    }
}