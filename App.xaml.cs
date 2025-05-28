using System.Configuration;
using System.Data;
using System.Windows;

namespace WeatherApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Разрешаем загрузку HTTPS изображений
        System.Net.ServicePointManager.SecurityProtocol |=
            System.Net.SecurityProtocolType.Tls12 |
            System.Net.SecurityProtocolType.Tls13;

        base.OnStartup(e);
    }
}

