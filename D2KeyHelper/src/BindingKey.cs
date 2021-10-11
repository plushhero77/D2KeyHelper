using NativeWin32.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class BindingKey : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BindingKey()
        {

        }

        public BindingKey(string _key = "Default_Key", VirtualKeyShort _value = VirtualKeyShort.F1, WM_WPARAM _eventParam = WM_WPARAM.WM_KEYUP, bool _isMacros = false)
        {
            Key = _key;
            Value = _value.ToString();
            EventParam = _eventParam.ToString(); ;
            IsMarcos = _isMacros;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string EventParam { get; set; }
        public bool IsMarcos { get; set; }

    }
}
