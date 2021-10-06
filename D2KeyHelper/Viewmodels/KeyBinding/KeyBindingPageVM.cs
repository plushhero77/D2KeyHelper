using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System;
using NativeWin32.Enums;
using System.Windows.Input;
using System.Linq;

namespace D2KeyHelper.Viewmodels.KeyBinding
{
    public class KeyBindingPageVM : BindableBase
    {

        public HookService HookService { get; }
        public static string[] Keys => Enum.GetNames(typeof(VirtualKeyShort));
        public static string[] Events => Enum.GetNames(typeof(WM_WPARAM));

        public KeyBindingPageVM(HookService _hookService)
        {
            HookService = _hookService;
        }

        public ICommand DeleteBinding => new DelegateCommand<string>(key =>
        {
            HookService.BindingKeyCollection.Remove(HookService.BindingKeyCollection.Where(x => x.Key == key).FirstOrDefault());
        });
        public ICommand AddBinding => new DelegateCommand<string>(key =>
        {
            HookService.BindingKeyCollection.Add(new BindingKey(key));
        });
    }
}
