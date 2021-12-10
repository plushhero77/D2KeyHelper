using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace D2KeyHelper.Services
{
    public class WatcherService
    {

        public Process GameProcess { get; private set; }
        public int Timeout { get; set; }
        public bool UseWatcher { get; set; }

        public bool StartWatcher()
        {
            throw new NotImplementedException();
        }
        public  bool StopWather()
        {
            throw new NotImplementedException();
        }
        
    }
}
