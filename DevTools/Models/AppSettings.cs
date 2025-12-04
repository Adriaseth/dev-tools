namespace DevTools.Models
{
    public class AppSettings
    {
        public double StandMinutes { get; set; } = 30;
        public double SitMinutes { get; set; } = 30;
        public bool DimScreen { get; set; } = true;
        public double DimAmount { get; set; } = 0.5;
    }
}
