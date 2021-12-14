using D2KeyHelper.src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace D2KeyHelper.Services
{
    public class ProfileService : INotifyPropertyChanged
    {
        private readonly string pathToProfDir = Path.Combine(Directory.GetCurrentDirectory(), "Profiles");
        private readonly SettingsService settingsService;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Profile> Profiles { get; set; }
        public Profile CurrentProfile { get; set; }

        public ProfileService(SettingsService _settingsService)
        {
            Initialization();
            InitProfListAsync();
            this.settingsService = _settingsService;
        }

        private void Initialization()
        {
            Profiles = new();
            this.PropertyChanged += new PropertyChangedEventHandler(delegate (object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "CurrentProfile")
                {
                    settingsService.Settings.LastProfileName = CurrentProfile.Name;
                }
            });
        }
        private async void InitProfListAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(pathToProfDir))
                    {
                        _ = Directory.CreateDirectory(pathToProfDir);
                    }

                    string[] files = Directory.GetFiles(pathToProfDir);

                    foreach (string file in files)
                    {
                        var bytes = File.ReadAllBytes(file);
                        var prof = JsonSerializer.Deserialize<Profile>(bytes);

                        Profiles.Add(prof);
                        if (prof.Name == settingsService.Settings.LastProfileName)
                        {
                            CurrentProfile = prof;
                        }
                    }
                    if (Profiles.Count == 0)
                    {
                        var profile = new Profile();
                        Profiles.Add(profile);
                        CurrentProfile = profile;
                    }
                    else if (CurrentProfile == null)
                    {
                        CurrentProfile = Profiles[0];
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine("InitProfListAsync");
                    _ = MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
        public async void SaveAsync(Profile profile)
        {
            await Task.Run(() =>
            {
                try
                {
                    string path = Path.Combine(pathToProfDir, profile.Name + ".profile");
                    byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(profile);
                    File.WriteAllBytes(path, bytes);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("AddAsync");
                    _ = MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }


    }
}
