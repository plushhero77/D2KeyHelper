using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class UserSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ExeFilePath { get; set; }
        public bool IsKeyUpEvent { get; set; }
        public string  LastProfileName{ get; set; }

    }
}
