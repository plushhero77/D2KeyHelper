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
    public class EditProfileVM : BindableBase, IViewModelBase
    {
        private readonly IProfileService _profileService;
        private readonly IWindowManagmentService _windowService;
        private readonly bool _isNew;

        public string[] CB_itemsCollection => Enum.GetNames(typeof(NativeWin32.Enums.VirtualKeyShort));
        public Profile EditableProfile { get; set; }
        public EditProfileVM(IProfileService profileService, IWindowManagmentService windowService, bool isNew = false)
        {
            _profileService = profileService;
            _windowService = windowService;
            _isNew = isNew;
            EditableProfile = isNew ? new Profile() : _profileService.CurrentProfile?.Clone();
        }

        public DelegateCommand AddBinding => new DelegateCommand(() =>
        {
            string str = "User Skill " + (EditableProfile.KeyBindingCollection.Count + 1);
            EditableProfile.KeyBindingCollection.Add(new BindingPair(str));
        });
        public DelegateCommand SaveProfile => new(() =>
        {
            if (!_isNew)
            {
                _profileService.EditCurrProfile(EditableProfile);
                _windowService.CloseViewModel<EditProfileVM>();
                Debug.WriteLine("profilesEdited");
            }
            else
            {
                if (_profileService.AddNewProfile(EditableProfile))
                {
                    Debug.WriteLine("Added new Profile");
                    _windowService.CloseViewModel<EditProfileVM>();
                }
            }
        });
        public DelegateCommand<BindingPair> DeleteBinding => new(pair =>
        {
            EditableProfile.KeyBindingCollection.Remove(pair);
        });

    }
}
