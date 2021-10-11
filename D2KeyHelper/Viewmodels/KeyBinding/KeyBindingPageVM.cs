using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System;
using NativeWin32.Enums;
using System.Windows.Input;
using System.Linq;
using D2KeyHelper.src;
using System.Diagnostics;

namespace D2KeyHelper.Viewmodels.KeyBinding
{
    public class KeyBindingPageVM : BindableBase
    {
        public ProfileService ProfileService { get; }

        public static string[] Keys => Enum.GetNames(typeof(VirtualKeyShort));
        public static string[] Events => Enum.GetNames(typeof(WM_WPARAM));

        public KeyBindingPageVM(ProfileService _profileService)
        {
            ProfileService = _profileService;
        }

        public ICommand DeleteBinding => new DelegateCommand<string>(key =>
        {
            BindingKey keyBinding = ProfileService.CurrentProfile.BindingKeysCollection.FirstOrDefault(x => x.Key == key);
            if (keyBinding != null) { _ = ProfileService.CurrentProfile.BindingKeysCollection.Remove(keyBinding); }
            else { Trace.WriteLine("Delete error"); }
        });
        public ICommand AddBinding => new DelegateCommand<string>(key =>
        {
            ProfileService.CurrentProfile.BindingKeysCollection.Add(new BindingKey(key));
        });
    }
}
