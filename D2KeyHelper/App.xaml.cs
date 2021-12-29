using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using D2KeyHelper.Services;
using D2KeyHelper.Viewmodels;

namespace D2KeyHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ViewMapper.MapWindowType<MainVM, MainWindow>();
            ViewMapper.MapWindowType<EditProfileVM, EditProfileWindow>();
        }
    }
}
