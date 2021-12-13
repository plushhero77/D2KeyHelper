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
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
        public Dictionary<string, Profile> Profiles { get; set; }
        public KeyValuePair<string, Profile> CurrentProfile { get; set; }

        public ProfileService()
        {
            Initialization();
            InitProfListAsync();
        }

        private void Initialization()
        {
            CurrentProfile = new();
            //PropertyChanged += new PropertyChangedEventHandler(delegate (object sender, PropertyChangedEventArgs e)
            //{
            //    if (e.PropertyName == "CurrentProfile")
            //    {
            //        CurrentProfile.Value.LastUpdateTime = DateTime.Now;

            //        _ = Task.Run(() =>
            //        {
            //            try
            //            {
            //                byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(CurrentProfile.Value);
            //                File.WriteAllBytes(CurrentProfile.Key, bytes);
            //            }
            //            catch (Exception ex)
            //            {
            //                Debug.WriteLine("initialization");
            //                _ = MessageBox.Show(ex.Message , ex.HResult.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            //            }
            //        });
            //    }
            //});
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
                        return;
                    }

                    string[] files = Directory.GetFiles(pathToProfDir);

                    foreach (string file in files)
                    {
                        byte[] bytes = File.ReadAllBytes(file);
                        Profiles.Add(file, JsonSerializer.Deserialize<Profile>(bytes));
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
                    //Check Dictionary
                    Profiles.Add(path, profile);
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
