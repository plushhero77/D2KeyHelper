using D2KeyHelper.src;
using DevExpress.Mvvm;
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
    public class ProfileService : BindableBase
    {
        private readonly string pathToProfDir = Path.Combine(Directory.GetCurrentDirectory(), "Profiles");
        private readonly SettingsService settingsService;

        public Dictionary<string, Profile> ProfilesDictionary { get; set; }
        public Profile CurrentProfile { get; set; }
        public ProfileService(SettingsService _settingsService)
        {
            Initialization();
            InitProfListAsync();
            this.settingsService = _settingsService;
        }

        private void Initialization()
        {
            ProfilesDictionary = new();
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

                    foreach (string filePath in files)
                    {
                        var bytes = File.ReadAllBytes(filePath);
                        var profile = JsonSerializer.Deserialize<Profile>(bytes);

                        ProfilesDictionary.Add(filePath, profile);
                        if (profile.Name == settingsService.Settings.LastProfileName)
                        {
                            CurrentProfile = profile;
                        }
                    }
                    if (ProfilesDictionary.Count == 0)
                    {
                        var profile = new Profile();
                        ProfilesDictionary.Add("new", profile);
                        CurrentProfile = profile;
                    }
                    else if (CurrentProfile == null)
                    {
                        CurrentProfile = ProfilesDictionary.First().Value;
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine("InitProfListAsync");
                    _ = MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
        public async Task<bool> SaveToFileAsync(Profile profile)
        {
            CurrentProfile.Name = profile.Name;
            CurrentProfile.KeyBindingCollection = profile.KeyBindingCollection;
            CurrentProfile.LastUpdateTime = DateTime.Now;

            return await Task<bool>.Factory.StartNew(() =>
            {
                bool taskResult = false;
                try
                {
                    string path = ProfilesDictionary.Where(x => x.Value == CurrentProfile).First().Key;

                    if (path == "new")
                    {
                        path = Path.Combine(pathToProfDir, CurrentProfile.Name + ".profile");
                        ProfilesDictionary.Remove("new");
                        ProfilesDictionary.Add(path, CurrentProfile);
                    }

                    byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(CurrentProfile);
                    File.WriteAllBytes(path, bytes);
                    return taskResult = true;
                }
                catch (ArgumentException)
                {
                    _ = MessageBox.Show("Файл и/или профиль с таким именем уже существует");
                    return taskResult;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("AddAsync");
                    _ = MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    return taskResult;
                }


            });
        }
        public void AddNewProfile()
        {
            Profile profile = new();
            ProfilesDictionary.Add("new", profile);
            CurrentProfile = profile;
        }


    }
}
