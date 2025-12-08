using DevTools.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var app = (App)Application.Current;
            if (!app.IsExit)
            {
                e.Cancel = true;
                this.Hide();
            }

            base.OnClosing(e);
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
                Navigate(btn.Tag.ToString());
        }

        private void Navigate(string? tag)
        {
            MainContent.Content = tag switch
            {
                "Home" => App.Host.Services.GetRequiredService<HomeView>(),
                "Timer" => App.Host.Services.GetRequiredService<TimerView>(),
                "Water" => App.Host.Services.GetRequiredService<WaterView>(),
                _ => App.Host.Services.GetRequiredService<HomeView>()
            };
        }
    }
}