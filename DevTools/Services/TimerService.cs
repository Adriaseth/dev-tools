using DevTools.Enums;
using DevTools.Interfaces;
using DevTools.Models;
using System.Windows.Threading;

namespace DevTools.Services
{
    public class TimerService
    {
        private readonly ISettingsService _settingsService;
        public event Action? Tick;
        public event Action? TimerEnd;
        public TimeSpan Remaining { get; private set; }
        public TimeSpan StandTimer { get; private set; }
        public TimeSpan SitTimer { get; private set; }
        private readonly DispatcherTimer _timer;
        public TimerModel CurrentModel { get; private set; }

        private AppSettings Settings => _settingsService.Settings;

        public TimerService(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            StandTimer = TimeSpan.FromMinutes(Settings.StandMinutes);
            SitTimer = TimeSpan.FromMinutes(Settings.SitMinutes);
            CurrentModel = TimerModel.Stand;
            Remaining = StandTimer;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => OnTick();
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public void StopTimer()
        {
            _timer.Stop();
        }

        private void OnTick()
        {
            if (Remaining.TotalSeconds > 0)
            {
                Remaining = Remaining.Add(TimeSpan.FromSeconds(-1));
            }
            else
            {
                TimerEnd?.Invoke();
                _timer.Stop();
                Remaining = CurrentModel == TimerModel.Stand ? SitTimer : StandTimer;
                CurrentModel = CurrentModel switch
                {
                    TimerModel.Stand => TimerModel.Sit,
                    TimerModel.Sit => TimerModel.Stand,
                    _ => CurrentModel
                };

                _timer.Start();
            }

            Tick?.Invoke();
        }

        public void AddOrSubtractFromTimer(TimerAction action, TimerModel model, int amountOfMinutes)
        {
            switch (action)
            {
                case TimerAction.Add when model == TimerModel.Stand:
                    StandTimer = StandTimer.Add(TimeSpan.FromMinutes(amountOfMinutes));
                    Settings.StandMinutes = StandTimer.TotalMinutes;
                    break;
                case TimerAction.Subtract when model == TimerModel.Stand:
                    StandTimer = StandTimer.Subtract(TimeSpan.FromMinutes(amountOfMinutes));
                    Settings.StandMinutes = StandTimer.TotalMinutes;
                    break;
                case TimerAction.Add when model == TimerModel.Sit:
                    SitTimer = SitTimer.Add(TimeSpan.FromMinutes(amountOfMinutes));
                    Settings.SitMinutes = SitTimer.TotalMinutes;
                    break;
                case TimerAction.Subtract when model == TimerModel.Sit:
                    SitTimer = SitTimer.Subtract(TimeSpan.FromMinutes(amountOfMinutes));
                    Settings.SitMinutes = SitTimer.TotalMinutes;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            _settingsService.Save();
        }
    }
}
