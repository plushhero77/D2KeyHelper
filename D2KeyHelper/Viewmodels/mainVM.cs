using NativeWin32;
using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Timers;
using System.Windows.Controls;
using D2KeyHelper.Pages;
using System.Windows;

namespace D2KeyHelper.Viewmodels
{
    public class mainVM : BindableBase
    {
        public HookService HookService { get; }
        private readonly PageNavigationService navigationService;
        public Page CurrentPage { get; set; }

        public mainVM(HookService _hookService, PageNavigationService _navigationService)
        {
            this.HookService = _hookService;
            this.navigationService = _navigationService;
            _navigationService.OnPageChanged += _navigationService_OnPageChanged;
        }

        private void _navigationService_OnPageChanged(Page page) => CurrentPage = page;


        public ICommand SetHook => new DelegateCommand(() =>
        {
            var d2Proc = Process.GetProcessesByName("D2R");
            HookService.SetHook(d2Proc[0]);
        });
        public ICommand OpenKeyBindingPage => new DelegateCommand(() =>
        {
            navigationService.Navigate(new KeyBindingPage());
        });
       
    }
}
