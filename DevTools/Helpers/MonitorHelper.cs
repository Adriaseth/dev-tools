using System.Runtime.InteropServices;
using System.Windows;

namespace DevTools.Helpers
{
    public static class MonitorHelper
    {
        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        private const uint MONITOR_DEFAULTTONEAREST = 2;

        public struct POINT { public int X; public int Y; }
        public struct RECT { public int Left, Top, Right, Bottom; }

        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        public static List<Rect> GetAllMonitors()
        {
            var result = new List<Rect>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (h, dc, ref r, d) =>
            {
                result.Add(new Rect(
                    r.Left,
                    r.Top,
                    r.Right - r.Left,
                    r.Bottom - r.Top));
                return true;
            }, IntPtr.Zero);

            return result;
        }

        public static Rect GetActiveMonitor()
        {
            GetCursorPos(out var p);
            var hMonitor = MonitorFromPoint(p, MONITOR_DEFAULTTONEAREST);
            var info = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            GetMonitorInfo(hMonitor, ref info);

            return new Rect(
                info.rcMonitor.Left, 
                info.rcMonitor.Top,
                info.rcMonitor.Right - info.rcMonitor.Left,
                info.rcMonitor.Bottom - info.rcMonitor.Top);
        }
    }
}
