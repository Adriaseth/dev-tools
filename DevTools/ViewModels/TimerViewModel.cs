using DevTools.Enums;
using DevTools.Services;
using System.ComponentModel;
using System.Windows;
using DevTools.Helpers;
using DevTools.Views;

namespace DevTools.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private readonly TimerService _timerService;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string StandText => _timerService.StandTimer.ToString();
        public string SitText => _timerService.SitTimer.ToString();
        public string RemainingText => _timerService.Remaining.ToString();

        public TimerViewModel(TimerService timerService)
        {
            _timerService = timerService;
            _timerService.Tick += OnTimerTick;
            _timerService.TimerEnd += OnTimerEnd;
        }

        public void StartTimer()
        {
            _timerService.StartTimer();
        }

        public void AddOrSubtractFromTimer(TimerAction action, TimerModel model, int amountOfMinutes)
        {
            _timerService.AddOrSubtractFromTimer(action, model, amountOfMinutes);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StandText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SitText)));
        }

        private void OnTimerTick()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingText)));
        }

        private void OnTimerEnd()
        {
            var nextAction = _timerService.CurrentModel == TimerModel.Stand ? "Sit down!" : "Stand up!";
            ShowOverlay(nextAction);
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
    }
}
