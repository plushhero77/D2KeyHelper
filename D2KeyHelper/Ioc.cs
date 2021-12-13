using D2KeyHelper.Services;
using D2KeyHelper.Viewmodels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0058,IDE0022

namespace D2KeyHelper
{
    public static class Ioc
    {
        private static ServiceProvider _provider;

        static Ioc()
        {
            ServiceCollection services = new();

            services.AddSingleton<MainVM>();
            services.AddTransient<EditProfileVM>();


            services.AddSingleton<HookService>();
            services.AddSingleton<ProfileService>();
            services.AddSingleton<SettingsService>();
            _provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _provider.GetRequiredService<T>();
    }
}
