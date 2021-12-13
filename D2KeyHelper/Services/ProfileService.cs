using D2KeyHelper.src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace D2KeyHelper.Services
{
    public class ProfileService : INotifyPropertyChanged
    {
        public ObservableCollection<Profile> Profiles { get; private set; } = new();
        public Profile CurrentProfile { get; set; }

        private readonly string profDir = Path.Combine(Directory.GetCurrentDirectory(), "Profiles");
        private readonly string ext = ".profile";

        public event PropertyChangedEventHandler PropertyChanged;

        public ProfileService()
        {
            Initialization();
        }

        private void Initialization()
        {
            CurrentProfile.PropertyChanged += new PropertyChangedEventHandler(delegate (object sender, PropertyChangedEventArgs e) { Add(CurrentProfile); });

            if (!Directory.Exists(profDir)) { _ = Directory.CreateDirectory(profDir); return; }

            string[] profiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Profiles"));

            if (profiles.Length > 0)
            {
                foreach (var profile in profiles)
                {
                    byte[] bytes = File.ReadAllBytes(profile);
                    Profiles.Add(JsonSerializer.Deserialize<Profile>(bytes));
                }
            }
        }


        public void Add(Profile profile)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(profile);
            string path = Path.Combine(profDir, profile.Name + ext);
            File.WriteAllBytesAsync(path, bytes);
        }
        //public async void AddAsync(Profile profile) => await Task.Run(() => { Add(profile); });
        public void Delete(Profile profile)
        {
            var path = Path.Combine(profDir, profile.Name, ext);

            if (File.Exists(path)) { File.Delete(path); }

        }
        //public async void DeleteAsync(Profile profile) => await Task.Run(() => { Delete(profile); });

    }
}
