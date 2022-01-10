using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class Profile : BindableBase ,IClonealabe<Profile>
    {
        public ObservableCollection<BindingPair> KeyBindingCollection { get; set; } = new();
        public string Name { get; set; } = "New_Profile";
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        public Profile Clone()
        {
            var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this, this.GetType());
            Profile temp = System.Text.Json.JsonSerializer.Deserialize<Profile>(bytes);
            return temp;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
