using DevTools.Models;
using System.IO;
using System.Text.Json;

namespace DevTools.Services
{
    public static class SettingsService
    {
        private static readonly string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DevTools");
        private static readonly string FilePath = Path.Combine(Folder, "settings.json");

        public static AppSettings Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new AppSettings();

                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch (Exception ex)
            {
                return new AppSettings();
            }
        }

        public static void Save(AppSettings settings)
        {
            Directory.CreateDirectory(Folder);

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FilePath, json);
        }
    }
}
