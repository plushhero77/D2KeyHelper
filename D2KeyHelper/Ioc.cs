using D2KeyHelper.Services;
using D2KeyHelper.Viewmodels;
using D2KeyHelper.src.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace D2KeyHelper
{
    public static class Ioc
    {
        private static readonly ServiceProvider _provider;
        static readonly ServiceCollection services = new();

        static Ioc()
        {
            services.AddSingleton<MainVM>();
            services.AddScoped<EditProfileVM>();

            services.AddSingleton<IHookService, HookService>();
            services.AddSingleton<IProfileService, ProfileService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IWindowManagmentService, WndMngService>();

            _provider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _provider.GetRequiredService<T>();

        public static T ResolveScoped<T>()
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<T>();
            }
        }
    }
}
