using D2KeyHelper.Services;
using D2KeyHelper.src;
using D2KeyHelper.src.Interfaces;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace D2KeyHelper.Viewmodels
{
    public class EditProfileVM: BindableBase 
    {
        private readonly ProfileService _profileService;

        public string[] CB_itemsCollection => Enum.GetNames(typeof(NativeWin32.Enums.VirtualKeyShort));
        public Profile EditableProfile { get; set; }
        public EditProfileVM(ProfileService profileService)
        {
            _profileService = profileService;
            EditableProfile = _profileService.CurrentProfile.Clone();
        }

        public DelegateCommand AddBinding => new DelegateCommand(() =>
        {
            string str = "User Skill " + (EditableProfile.KeyBindingCollection.Count + 1);
            EditableProfile.KeyBindingCollection.Add(new BindingPair(str));
        });
        public DelegateCommand<Window> SaveProfile => new(wnd =>
        {
            if (_profileService.EditOrCreateProfile(EditableProfile))
            {
                wnd.Close();
            }
        });
        public DelegateCommand<BindingPair> DeleteBinding => new(pair =>
        {
            EditableProfile.KeyBindingCollection.Remove(pair);
        });

    }
}
