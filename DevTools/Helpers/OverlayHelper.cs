using DevTools.Views;
using System.Windows;
using System.Windows.Media.Animation;

namespace DevTools.Helpers
{
    public static class OverlayHelper
    {
        public static void CloseAllOverlays()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is PopupOverlayWindow || w is DimOverlayWindow)
                    w.FadeAndClose();
            }
        }

        private static void FadeAndClose(this Window window, double durationSeconds = 0.5)
        {
            var fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.2
            };

            fadeOut.Completed += (_, __) => window.Close();
            window.BeginAnimation(Window.OpacityProperty, fadeOut);
        }
    }
}
