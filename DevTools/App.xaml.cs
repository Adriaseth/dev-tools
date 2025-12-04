using System.Windows;
using DevTools.Models;
using DevTools.Services;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppSettings Settings { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Settings = SettingsService.Load();
        }
    }
}
