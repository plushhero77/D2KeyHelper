using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace D2KeyHelper.Viewmodels.KeyBinding
{
    public class KeyBindingPageVM : BindableBase
    {
        private readonly HookService hookService;
        public bool UseKeyUpEvent { get; set; }
        public bool UseKeyDownEvent { get; set; }
        public ObservableCollection<StackPanel> StackPanels { get; set; }
        public Dictionary<string, ConsoleKey> ConsoleKeys { get; set; }

        public KeyBindingPageVM(HookService _hookService)
        {
            hookService = _hookService;

            StackPanels = new ObservableCollection<StackPanel>();
            ConsoleKeys = new Dictionary<string, ConsoleKey>();

            Init();
        }

        private void Init()
        {
            ConsoleKey[] keys = new ConsoleKey[] { ConsoleKey.Escape, ConsoleKey.Enter, ConsoleKey.Tab, ConsoleKey.UpArrow, ConsoleKey.Add, ConsoleKey.LeftArrow };
            for (int i = 0; i < keys.Length; i++)
            {
                ConsoleKeys.Add($"User Skill {i}", keys[i]);
            }

        }

        public ICommand SetWParamToKeyUpEvent => new DelegateCommand<object>(obj =>
        {
            var wParam = Enum.Parse(typeof(NativeWin32.NativeWin32Enums.WM_WPARAM), obj.ToString());
            hookService.WM_WPARAM = (NativeWin32.NativeWin32Enums.WM_WPARAM)wParam;
            MessageBox.Show(hookService.WM_WPARAM.ToString());
        });
    }
}
