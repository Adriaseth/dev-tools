using DevTools.Helpers;
using System.Windows;
using System.Windows.Threading;

namespace DevTools.Views
{
    public abstract class OverlayBaseWindow : Window
    {
        private static readonly TimeSpan AutoCloseDelay = TimeSpan.FromSeconds(3);

        protected OverlayBaseWindow()
        {
            StartAutoCloseTimer();
        }

        private void StartAutoCloseTimer()
        {
            var timer = new DispatcherTimer
            {
                Interval = AutoCloseDelay
            };

            timer.Tick += (_, __) =>
            {
                timer.Stop();
                OverlayHelper.CloseAllOverlays();
            };

            timer.Start();
        }
    }
}
