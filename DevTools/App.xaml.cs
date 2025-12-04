using System.Windows;
using DevTools.Interfaces;
using DevTools.Models;
using DevTools.Services;
using DevTools.ViewModels;
using DevTools.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost Host { get; private set; }

        public App()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //App-wide services
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddSingleton<TimerService>();

                    //View models
                    services.AddSingleton<TimerViewModel>();

                    // Views
                    services.AddSingleton<HomeView>();
                    services.AddSingleton<TimerView>();
                    services.AddSingleton<WaterView>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Host.Start();
            base.OnStartup(e);
            //Settings = SettingsService.Load();
            //TimerService = new TimerService();
        }
    }
}
