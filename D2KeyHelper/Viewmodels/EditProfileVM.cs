using D2KeyHelper.Services;
using D2KeyHelper.src;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly WindowManagmentService _windowService;
        private ProfileService _profileService;

        public string[] CB_itemsCollection => Enum.GetNames(typeof(NativeWin32.Enums.VirtualKeyShort));
        public Profile EditableProfile { get; set; }
        public EditProfileVM(ProfileService profileService, WindowManagmentService windowService)
        {
            _profileService = profileService;
            _windowService = windowService;
            EditableProfile = _profileService.CurrentProfile.Clone();
        }

        public DelegateCommand AddBinding => new DelegateCommand(() =>
        {
            string str = "User Skill " + (EditableProfile.KeyBindingCollection.Count + 1);
            EditableProfile.KeyBindingCollection.Add(new BindingPair(str));
        });
        public DelegateCommand<Window> SaveProfile => new(wnd =>
        {
            _profileService.EditProfile(EditableProfile);
            wnd.Close();
        });

    }
}
