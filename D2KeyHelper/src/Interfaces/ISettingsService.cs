using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src.Interfaces
{
    public interface ISettingsService
    {
        public UserSettings Settings { get; }
        public void Load();
        public void Save();

    }
}
