using System.Windows;
using System.Windows.Controls;
using DevTools.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Menu.SelectedIndex = 0;
        }

        private void Menu_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Menu.SelectedItem is not ListBoxItem item)
                return;

            switch (item.Tag)
            {
                case "Home":
                    var homeView = App.Host.Services.GetRequiredService<HomeView>();
                    MainContent.Content = homeView;
                    break;
                case "Timer":
                    var timerView = App.Host.Services.GetRequiredService<TimerView>();
                    MainContent.Content = timerView;
                    break;
                case "Water":
                    var waterView = App.Host.Services.GetRequiredService<WaterView>();
                    MainContent.Content = waterView;
                    break;

            }
        }
    }
}