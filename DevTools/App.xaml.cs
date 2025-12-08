using DevTools.Interfaces;
using DevTools.Services;
using DevTools.ViewModels;
using DevTools.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost Host { get; private set; } = null!;
        public static NotifyIcon TrayIcon { get; private set; } = null!;

        public bool IsExit { get; private set; }

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
                .Build()!;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mutex = new Mutex(true, "DevTools_SingleInstanceApp", out var isNewInstance);
            if (!isNewInstance)
            {
                MessageBox.Show("The application is already running.", "Already running", MessageBoxButton.OK, MessageBoxImage.Warning);
                Shutdown();
                return;
            }

            Host.Start();
            base.OnStartup(e);

            TrayIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("Assets/icon.ico"),
                Visible = true,
                Text = "DevTools"
            };

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Exit", null, (s, e) =>
            {
                IsExit = true;
                TrayIcon.Visible = false;
                Current.Shutdown();
            });

            TrayIcon.ContextMenuStrip = contextMenuStrip;

            TrayIcon.DoubleClick += (s, args) =>
            {
                var mainWindow = Current.MainWindow;
                if (mainWindow == null) return;

                if (mainWindow is { WindowState: WindowState.Minimized })
                    mainWindow.WindowState = WindowState.Normal;


                mainWindow.Show();
                mainWindow.Activate();
                mainWindow.Topmost = true;
                mainWindow.Topmost = false;
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            TrayIcon.Dispose();
            base.OnExit(e);
        }
    }
}
