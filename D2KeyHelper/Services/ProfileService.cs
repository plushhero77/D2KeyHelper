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
        
        public ObservableDictionary<string, Profile> ProfilesDictionary { get; set; }
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
                        ProfilesDictionary.Add(null, profile);
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
        private async Task<bool> SaveToFileAsync()
        {
            CurrentProfile.LastUpdateTime = DateTime.Now;

            return await Task<bool>.Factory.StartNew(() =>
            {
                bool taskResult = false;
                try
                {
                    string path = ProfilesDictionary.Where(x => x.Value == CurrentProfile).FirstOrDefault().Key;

                    if (path == null || path == "new")
                    {
                        ProfilesDictionary.Remove(path);
                        path = Path.Combine(pathToProfDir, CurrentProfile.Name + ".profile");
                        ProfilesDictionary[path] = CurrentProfile;
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
        public void AddNewProfile(Profile profile)
        {
            ProfilesDictionary["new"] = profile;
            CurrentProfile = profile;
        }
        public void EditProfile(Profile profile)
        {
            try
            {
                var key = ProfilesDictionary.First(x => x.Value == CurrentProfile).Key;
                ProfilesDictionary.Remove(key);
                ProfilesDictionary[key] = profile;
                CurrentProfile = ProfilesDictionary[key];
                _ = SaveToFileAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteProfile()
        {
            throw new NotImplementedException();
        }
        public void CopyToNewProfile()
        {
            throw new NotImplementedException();
        }
    }
}
