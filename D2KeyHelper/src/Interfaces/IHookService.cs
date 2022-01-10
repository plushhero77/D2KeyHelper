using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper.src.Interfaces
{
    public interface IHookService
    {
        public bool IsHookSet { get; }
        public bool SetHook(int id);
        public bool DeleteHook();

    }
}
