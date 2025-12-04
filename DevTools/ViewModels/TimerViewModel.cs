using DevTools.Enums;
using DevTools.Services;
using System.ComponentModel;

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
    }
}
