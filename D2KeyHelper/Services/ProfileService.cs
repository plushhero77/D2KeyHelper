using D2KeyHelper.src;
using D2KeyHelper.src.Interfaces;
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
    public class ProfileService : BindableBase, IProfileService
    {
        private readonly string pathToProfDir = Path.Combine(Directory.GetCurrentDirectory(), "Profiles");
        private readonly ISettingsService settingsService;
        private ObservableDictionary<string, Profile> profilesDictionary;

        public Profile CurrentProfile { get; set; }
        public ICollection<Profile> ProfilesCollection => profilesDictionary.Values;

        public ProfileService(ISettingsService _settingsService)
        {
            Initialization();
            InitProfListAsync();
            this.settingsService = _settingsService;
        }

        private void Initialization()
        {
            profilesDictionary = new();
            this.PropertyChanged += new PropertyChangedEventHandler(delegate (object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "CurrentProfile")
                {
                    settingsService.Settings.LastProfileName = CurrentProfile != null ? CurrentProfile.Name : null;
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

                        profilesDictionary.Add(filePath, profile);
                        if (profile.Name == settingsService.Settings.LastProfileName)
                        {
                            CurrentProfile = profile;
                        }
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
                    string path = profilesDictionary.Where(x => x.Value == CurrentProfile).FirstOrDefault().Key;
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
        public bool AddNewProfile(Profile profile)
        {
            var path = Path.Combine(pathToProfDir, profile.Name + ".profile");

            if (profilesDictionary.Values.Select(x => x.Name).ToArray().Contains(profile.Name))
            {
                MessageBox.Show($"Name {profile.Name} is already exist!");
                return false;
            }
            else if (profilesDictionary.Keys.Contains(path))
            {
                MessageBox.Show($"File with name {profile.Name} is already exist!");
                return false;
            }

            profilesDictionary[path] = profile;
            CurrentProfile = profilesDictionary[path];
            _ = SaveToFileAsync();
            this.RaisePropertyChanged(nameof(ProfilesCollection));
            return true;
        }
        public bool EditCurrProfile(Profile profile)
        {
            try
            {
                var key = profilesDictionary.FirstOrDefault(x => x.Value == CurrentProfile).Key;
                profilesDictionary.Remove(key);
                profilesDictionary[key] = profile;
                CurrentProfile = profilesDictionary[key];
                _ = SaveToFileAsync();
                this.RaisePropertyChanged(nameof(ProfilesCollection));
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteProfile(Profile profile)
        {

            if (profile == null)
                throw new ArgumentNullException("Profile cannot be null!");

            var valuePair = profilesDictionary.FirstOrDefault(x => x.Value == profile);
            var index = profilesDictionary.GetIndex(valuePair);

            if (!profilesDictionary.Remove(valuePair.Key))
                throw new ArgumentException($" Profile {profile} ,not found on {profilesDictionary}");


            if (File.Exists(valuePair.Key))
                File.Delete(valuePair.Key);

            index = profilesDictionary.Count == index ? index - 1 : index;
            CurrentProfile = index >= 0 ? profilesDictionary[index] : null;

            this.RaisePropertiesChanged(nameof(ProfilesCollection));

            return true;



        }
        public Profile CopyToNewProfile(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
