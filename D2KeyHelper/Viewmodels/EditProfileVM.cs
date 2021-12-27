using D2KeyHelper.Services;
using D2KeyHelper.src;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace D2KeyHelper.Viewmodels
{
    public class EditProfileVM : BindableBase
    {
        public ProfileService ProfileService { get; }
        public string[] CB_itemsCollection => Enum.GetNames(typeof(NativeWin32.Enums.VirtualKeyShort));
        public Visibility SaveStatus { get; set; }
        public Profile EditableProfile { get; set; }

        public DelegateCommand AddBinding => new DelegateCommand(() =>
        {
            string str = "User Skill " + (EditableProfile.KeyBindingCollection.Count + 1);
            EditableProfile.KeyBindingCollection.Add(new BindingPair(str));
        });
        public DelegateCommand<Window> SaveProfile => new DelegateCommand<Window>(async (window) =>
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            if (await ProfileService.SaveToFileAsync(EditableProfile))
            {
                SaveStatus = Visibility.Visible;

                var tim = new Timer(1000);
                tim.Elapsed += new ElapsedEventHandler(delegate (object sender, ElapsedEventArgs e)
                {
                    ((Timer)sender).Stop();
                    dispatcher.Invoke(() => { window.DialogResult = true; });
                });
                tim.Start();
            }
        });

        public EditProfileVM(ProfileService profileService)
        {
            ProfileService = profileService;
            SaveStatus = Visibility.Collapsed;
            EditableProfile = ProfileService.CurrentProfile.Clone();
        }


    }
}
