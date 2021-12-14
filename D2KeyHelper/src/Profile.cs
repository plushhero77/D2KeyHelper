﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class Profile :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BindingPair> KeyBindingCollection { get; set; } = new();
        public string Name { get; set; } = "Defalut";
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
