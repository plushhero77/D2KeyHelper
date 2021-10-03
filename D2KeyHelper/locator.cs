using D2KeyHelper.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper
{
    public class locator
    {
        public mainVM mainVM => Ioc.Resolve<mainVM>();
    }
}
