using DevTools.Models;
using DevTools.Services;
using System.Windows;
using System.Windows.Threading;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan _remaining = TimeSpan.FromMinutes(0);
        private TimeSpan _standTimer;
        private TimeSpan _sitTimer;
        private readonly DispatcherTimer _timer;
        private Model _currentModel = Model.Stand;
        private AppSettings _settings;

        private enum Model
        {
            Stand,
            Sit
        }

        private enum TimerAction
        {
            Add,
            Subtract
        }

        public MainWindow()
        {
            InitializeComponent();
            _settings = SettingsService.Load();

            _standTimer = TimeSpan.FromMinutes(_settings.StandMinutes);
            _sitTimer = TimeSpan.FromMinutes(_settings.SitMinutes);

            lblTimerStand.Content = _standTimer.ToString();
            lblTimerSit.Content = _sitTimer.ToString();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {

            if (_remaining.TotalSeconds > 0)
            {
                _remaining = _remaining.Add(TimeSpan.FromSeconds(-1));
                lblTimer.Content = _remaining.ToString();
            }
            else
            {
                _timer.Stop();
                _remaining = _currentModel == Model.Stand ? _standTimer : _sitTimer;
                switch (_currentModel)
                {
                    case Model.Stand:
                        _currentModel = Model.Sit;
                        break;
                    case Model.Sit:
                        _currentModel = Model.Stand;
                        break;
                }
                _timer.Start();
            }
        }

        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void btnStandAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrSubtractFromTimer(TimerAction.Add, Model.Stand, 1);
        }

        private void btnStandSubtract_Click(object sender, RoutedEventArgs e)
        {
            AddOrSubtractFromTimer(TimerAction.Subtract, Model.Stand, 1);
        }

        private void btnSitAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrSubtractFromTimer(TimerAction.Add, Model.Sit, 1);
        }

        private void btnSitSubtract_Click(object sender, RoutedEventArgs e)
        {
            AddOrSubtractFromTimer(TimerAction.Subtract, Model.Sit, 1);
        }

        private void AddOrSubtractFromTimer(TimerAction action, Model model, int amountOfMinutes)
        {
            switch (action)
            {
                case TimerAction.Add when model == Model.Stand:
                    _standTimer = _standTimer.Add(TimeSpan.FromMinutes(amountOfMinutes));
                    lblTimerStand.Content = _standTimer.ToString();
                    _settings.StandMinutes = _standTimer.TotalMinutes;
                    break;
                case TimerAction.Subtract when model == Model.Stand:
                    _standTimer = _standTimer.Subtract(TimeSpan.FromMinutes(amountOfMinutes));
                    lblTimerStand.Content = _standTimer.ToString();
                    _settings.StandMinutes = _standTimer.TotalMinutes;
                    break;
                case TimerAction.Add when model == Model.Sit:
                    _sitTimer = _sitTimer.Add(TimeSpan.FromMinutes(amountOfMinutes));
                    lblTimerSit.Content = _sitTimer.ToString();
                    _settings.SitMinutes = _sitTimer.TotalMinutes;
                    break;
                case TimerAction.Subtract when model == Model.Sit:
                    _sitTimer = _sitTimer.Subtract(TimeSpan.FromMinutes(amountOfMinutes));
                    lblTimerSit.Content = _sitTimer.ToString();
                    _settings.SitMinutes = _sitTimer.TotalMinutes;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            SettingsService.Save(_settings);
        }
    }
}