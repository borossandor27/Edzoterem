using System.Windows;
using Edzoterem.Desktop.Services;
using Edzoterem.Desktop.ViewModels;
using Edzoterem.Desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edzoterem.Desktop;

public partial class App : Application
{
    private IServiceProvider _services = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var apiBaseUrl = configuration["ApiBaseUrl"] ?? "https://localhost:5001/";

        var services = new ServiceCollection();

        services.AddSingleton<SessionState>();
        services.AddHttpClient<ApiClient>(client => client.BaseAddress = new Uri(apiBaseUrl));

        services.AddTransient<AuthService>();
        services.AddTransient<DolgozokService>();
        services.AddTransient<TagokService>();
        services.AddTransient<BerletekService>();
        services.AddTransient<CsoportosFoglalkozasokService>();

        services.AddTransient<LoginViewModel>();
        services.AddSingleton<DolgozokListViewModel>();
        services.AddSingleton<TagokListViewModel>();
        services.AddSingleton<BerletekViewModel>();
        services.AddSingleton<CsoportosFoglalkozasokViewModel>();
        services.AddSingleton<MainViewModel>();

        services.AddTransient<LoginWindow>();
        services.AddSingleton<MainWindow>();

        _services = services.BuildServiceProvider();

        var loginWindow = _services.GetRequiredService<LoginWindow>();
        var bejelentkezveSikeresen = loginWindow.ShowDialog();

        if (bejelentkezveSikeresen != true)
        {
            Shutdown();
            return;
        }

        var mainWindow = _services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
