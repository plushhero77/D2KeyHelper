using D2KeyHelper.Services;
using D2KeyHelper.Viewmodels;
using D2KeyHelper.Viewmodels.Main;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2KeyHelper
{
    public static class Ioc
    {
        private static ServiceProvider _provider;

        static Ioc()
        {
            var services = new ServiceCollection();

            services.AddSingleton<mainVM>();
            services.AddSingleton<HookService>();

            _provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _provider.GetRequiredService<T>();
    }
}
