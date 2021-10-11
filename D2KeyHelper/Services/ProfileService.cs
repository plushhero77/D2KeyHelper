using D2KeyHelper.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace D2KeyHelper.Services
{
    public class ProfileService
    {
        private CharactreProfile currentProfile;
        private readonly string pathToProfFolder = Path.Combine(Directory.GetCurrentDirectory(), "Profiles");

        public ProfileService()
        {
            ProfileList = new List<CharactreProfile>();
            InitProfileList();
        }

        public CharactreProfile CurrentProfile
        {
            get => currentProfile;
            set
            {
                ConfigurationManager.AppSettings.Set("currentProfile", value.Name);
                if (currentProfile != null)
                {
                    currentProfile.OnProfileChanged -= Value_OnProfileChanged;
                }
                currentProfile = value;
                currentProfile.OnProfileChanged += Value_OnProfileChanged;
            }
        }
        public List<CharactreProfile> ProfileList { get; }
        public string PathToProfileFolder { get; set; }

#warning Убрать множественный вызов
        private void Value_OnProfileChanged()
        {
            string json = JsonSerializer.Serialize(currentProfile);
            using StreamWriter sw = File.CreateText(Path.Combine(pathToProfFolder, currentProfile.Name));
            sw.Write(json);
            sw.Close();
        }
        private bool InitProfileList()
        {
            string curProfName = ConfigurationManager.AppSettings.Get("currentProfile");
            Debug.WriteLine($"{currentProfile}");

            if (!Directory.Exists(pathToProfFolder)) { _ = Directory.CreateDirectory(pathToProfFolder); }

            foreach (string item in Directory.GetFiles(pathToProfFolder))
            {
                try
                {
                    string json = File.ReadAllText(Path.Combine(pathToProfFolder, item));
                    CharactreProfile profile = JsonSerializer.Deserialize<CharactreProfile>(json);
                    if (profile.Name == curProfName) { CurrentProfile = profile; }
                    ProfileList.Add(profile);
                }
                catch (JsonException ex)
                {
                    _ = MessageBox.Show(ex.Message, "Read profile Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }

            if (currentProfile == null)
            {
                CurrentProfile = new CharactreProfile();
                ProfileList.Add(CurrentProfile);
            }

            return true;

        }
    }
}
