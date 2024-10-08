using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Catser.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Catser.UI;

public partial class App : Application
{
    public static readonly string AppDataDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "catser");

    private static readonly string GlobalConfigFilePath;
    private static readonly string UserConfigFilePath;
    private static readonly string LocalConfigFilePath;

    static App()
    {
        Directory.CreateDirectory(AppDataDirPath);

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CATSER_CONFIG")))
        {
            UserConfigFilePath = AppDataDirPath;
        }
        else
        {
            UserConfigFilePath = Environment.GetEnvironmentVariable("CATSER_CONFIG")!;
        }

        GlobalConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Register all the services needed for the application to run
        var collection = new ServiceCollection();

        ConfigureServices(collection);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();


        //// Create the AutoSuspendHelper.
        //var filePath = Path.Combine(AppDataDirPath, "appstate.json");
        //var suspension = new AutoSuspendHelper(ApplicationLifetime);
        //RxApp.SuspensionHost.CreateNewAppState = () => new MainWindowViewModel();
        //RxApp.SuspensionHost.SetupDefaultSuspendResume(new NewtonsoftJsonSuspensionDriver(filePath));
        //suspension.OnFrameworkInitializationCompleted();

        //// Load the saved view model state.
        //var state = RxApp.SuspensionHost.GetAppState<MainWindowViewModel>();

        var mainViewModel = services.GetRequiredService<MainWindowViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                DataContext = mainViewModel,
            };
            //desktop.ShutdownRequested += Desktop_ShutdownRequested;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(LoadConfiguration());

        if (ApplicationLifetime is not null)
        {
            services.AddSingleton(ApplicationLifetime);
        }

        services.AddTransient<MainWindowViewModel>();
    }

    private IConfiguration LoadConfiguration()
    {
        var configBuilder = new ConfigurationBuilder();

        configBuilder
            .AddJsonFile(UserConfigFilePath, optional: true, reloadOnChange: true)
            .AddJsonFile("catser.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            ;

        return configBuilder.Build();
    }
}
