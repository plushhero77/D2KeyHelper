using D2KeyHelper.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace D2KeyHelper.Services
{
    public class SettingsService
    {
        public UserSettings Settings { get; private set; }
        private readonly string pathToSettings = Path.Combine(Directory.GetCurrentDirectory(), "UserSettings.set");

        public SettingsService()
        {
            Load();
            Settings.PropertyChanged += new PropertyChangedEventHandler(delegate (object sender, PropertyChangedEventArgs e) { Save(); });
        }

        private void Load()
        {
            if (File.Exists(pathToSettings))
            {
                var bytes = File.ReadAllBytes(pathToSettings);
                Settings = JsonSerializer.Deserialize<UserSettings>(new ReadOnlySpan<byte>(bytes));
            }
            else
            {
                Settings = new();
            }
        }
        private void Save()
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(Settings);
            File.WriteAllBytes(pathToSettings, bytes);

        }


    }
}
