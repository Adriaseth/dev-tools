using DevTools.Enums;
using DevTools.Helpers;
using DevTools.Services;
using DevTools.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevTools.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private readonly TimerService _timerService;
        private bool _isRunning;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string StandText => _timerService.StandTimer.ToString(@"hh\:mm\:ss");
        public string SitText => _timerService.SitTimer.ToString(@"hh\:mm\:ss");
        public string RemainingText => _timerService.Remaining.ToString(@"hh\:mm\:ss");
        public string StartStopText => IsRunning ? "Stop" : "Start";
        public Brush StartStopColor => new SolidColorBrush(IsRunning ? Colors.Red : Colors.Green);
        public string SitStandText => _timerService.CurrentModel == TimerModel.Stand ? "Zitten" : "Opstaan";

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                _isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotRunning)));
            }
        }

        public bool IsNotRunning => !IsRunning;

        public TimerViewModel(TimerService timerService)
        {
            _timerService = timerService;
            _timerService.Tick += OnTimerTick;
            _timerService.TimerEnd += OnTimerEnd;
        }

        public void StartStopTimer()
        {
            if (IsRunning)
            {
                _timerService.StopTimer();
                IsRunning = false;
            }
            else
            {
                _timerService.StartTimer();
                IsRunning = true;
            }

            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(IsNotRunning));
            OnPropertyChanged(nameof(StartStopText));
            OnPropertyChanged(nameof(StartStopColor));
        }

        public void AddOrSubtractFromTimer(TimerAction action, TimerModel model, int amountOfMinutes)
        {
            if (!IsRunning)
            {
                _timerService.AddOrSubtractFromTimer(action, model, amountOfMinutes);
                OnPropertyChanged(nameof(StandText));
                OnPropertyChanged(nameof(SitText));
                OnPropertyChanged(nameof(RemainingText));
            }
        }

        private void OnTimerTick()
        {
            OnPropertyChanged(nameof(RemainingText));
        }

        private void OnTimerEnd()
        {
            var nextAction = SitStandText;
            ShowOverlay(nextAction);
            IsRunning = false;
        }

        private readonly List<Window> _openOverlays = new();

        private void ShowOverlay(string message)
        {
            foreach (var w in _openOverlays)
                w.Close();

            _openOverlays.Clear();

            var monitos = MonitorHelper.GetAllMonitors();
            var active = MonitorHelper.GetActiveMonitor();

            foreach (var m in monitos)
            {
                var overlay = new DimOverlayWindow
                {
                    Left = m.Left,
                    Top = m.Top,
                    Width = m.Width,
                    Height = m.Height,
                    Opacity = 0.7
                };

                overlay.Show();
                _openOverlays.Add(overlay);
            }

            var popup = new PopupOverlayWindow(message)
            {
                Left = active.Left,
                Top = active.Top,
                Width = active.Width,
                Height = active.Height
            };

            popup.Show();
            _openOverlays.Add(popup);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
