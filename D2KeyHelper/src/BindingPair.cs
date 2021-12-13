using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src
{
    public class BindingPair
    {
        public BindingPair(string name,NativeWin32.Enums.VirtualKeyShort keyShort)
        {
            Name = name;
            KeyShort = keyShort;
        }
        public string Name { get; set; }
        public NativeWin32.Enums.VirtualKeyShort KeyShort { get; set; }
    }
}
