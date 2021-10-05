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
        public string[] MyProperty { get; set; }

        public KeyBindingPageVM(HookService _hookService)
        {
            HookService = _hookService;
            MyProperty = Enum.GetNames<VirtualKeyShort>();
        }
        
        public ICommand SetKeyBinding => new DelegateCommand<EBindedKeys>(obj => {
            
        });

        public ICommand FindKey => new DelegateCommand(()=> {
            
        });
        public ICommand SetWParamToKeyUpEvent => new DelegateCommand<object>(obj =>
        {
            var wParam = Enum.Parse(typeof(WM_WPARAM), obj.ToString());
            HookService.WM_WPARAM = (WM_WPARAM)wParam;
            MessageBox.Show(HookService.WM_WPARAM.ToString());
        });
    }
}
