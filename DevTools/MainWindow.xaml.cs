using System.Windows;
using System.Windows.Threading;

namespace SitStandApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan _remaining = TimeSpan.FromMinutes(30);
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if(_remaining.TotalSeconds > 0)
            {
                _remaining = _remaining.Add(TimeSpan.FromSeconds(-1));
                lblTimer.Content = _remaining.ToString();
            }
            else
            {
                _timer.Stop();
                lblTimer.Content = "00:00:00";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _remaining = _remaining.Add(TimeSpan.FromMinutes(1));
            lblTimer.Content = _remaining.ToString();
        }

        private void btnSubtract_Click(object sender, RoutedEventArgs e)
        {
            _remaining = _remaining.Subtract(TimeSpan.FromMinutes(1));
            lblTimer.Content = _remaining.ToString();
        }

        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }
    }
}