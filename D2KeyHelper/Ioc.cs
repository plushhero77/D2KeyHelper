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
        private static ServiceProvider _provider;
        static ServiceCollection services = new();

        static Ioc()
        {

            services.AddSingleton<MainVM>();
            services.AddScoped<EditProfileVM>();

            services.AddSingleton<HookService>();
            services.AddSingleton<ProfileService>();
            services.AddSingleton<SettingsService>();
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
