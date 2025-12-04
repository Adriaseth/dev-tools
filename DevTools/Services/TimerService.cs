using DevTools.Enums;
using DevTools.Models;
using System.Windows.Threading;

namespace DevTools.Services
{
    public class TimerService
    {
        public event Action? Tick;
        public TimeSpan Remaining { get; private set; }
        public TimeSpan StandTimer { get; private set; }
        public TimeSpan SitTimer { get; private set; }
        private readonly DispatcherTimer _timer;
        private TimerModel _currentModel = TimerModel.Stand;
        private readonly AppSettings _settings;

        public TimerService()
        {
            _settings = SettingsService.Load();

            StandTimer = TimeSpan.FromMinutes(_settings.StandMinutes);
            SitTimer = TimeSpan.FromMinutes(_settings.SitMinutes);

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
                _timer.Stop();
                Remaining = _currentModel == TimerModel.Stand ? StandTimer : SitTimer;
                _currentModel = _currentModel switch
                {
                    TimerModel.Stand => TimerModel.Sit,
                    TimerModel.Sit => TimerModel.Stand,
                    _ => _currentModel
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
                    _settings.StandMinutes = StandTimer.TotalMinutes;
                    break;
                case TimerAction.Subtract when model == TimerModel.Stand:
                    StandTimer = StandTimer.Subtract(TimeSpan.FromMinutes(amountOfMinutes));
                    _settings.StandMinutes = StandTimer.TotalMinutes;
                    break;
                case TimerAction.Add when model == TimerModel.Sit:
                    SitTimer = SitTimer.Add(TimeSpan.FromMinutes(amountOfMinutes));
                    _settings.SitMinutes = SitTimer.TotalMinutes;
                    break;
                case TimerAction.Subtract when model == TimerModel.Sit:
                    SitTimer = SitTimer.Subtract(TimeSpan.FromMinutes(amountOfMinutes));
                    _settings.SitMinutes = SitTimer.TotalMinutes;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            SettingsService.Save(_settings);
        }
    }
}
