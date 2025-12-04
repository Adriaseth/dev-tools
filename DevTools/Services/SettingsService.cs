using DevTools.Interfaces;
using DevTools.Models;
using System.IO;
using System.Text.Json;

namespace DevTools.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DevTools");
        private readonly string _filePath;
        public AppSettings Settings { get; private set; }

        public SettingsService()
        {
            _filePath = Path.Combine(_folder, "settings.json");
            Settings = Load();
        }

        private AppSettings Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new AppSettings();

                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch (Exception)
            {
                return new AppSettings();
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(_folder);

            var json = JsonSerializer.Serialize(Settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}
