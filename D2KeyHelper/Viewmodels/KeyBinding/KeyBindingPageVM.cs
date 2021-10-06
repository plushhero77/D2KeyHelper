using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using NativeWin32.Enums;
using System.Windows.Input;

namespace D2KeyHelper.Viewmodels.KeyBinding
{
    public class KeyBindingPageVM : BindableBase
    {
        public HookService HookService { get; }
        public string[] Keys { get
            {
              return  Enum.GetNames(typeof(VirtualKeyShort));
            } 
        }
        public string[] Events { get
            {
                return Enum.GetNames(typeof(WM_WPARAM));
            } }

        public KeyBindingPageVM(HookService _hookService)
        {
            HookService = _hookService;
        }
        
        public ICommand EditBinding => new DelegateCommand<string>(key => {
            HookService.EditKeyBinding(key, VirtualKeyShort.F1);
        });
        public ICommand DeleteBinding => new DelegateCommand<string>(key => {
            HookService.DeleteKeyBinding(key);
        });

        public ICommand AddBinding => new DelegateCommand<string>(key => {
            HookService.AddKeyBinding("test", VirtualKeyShort.F1);
        });
        public ICommand SetWParam => new DelegateCommand<object>(obj =>
        {

        });
    }
}
