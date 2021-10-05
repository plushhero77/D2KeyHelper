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
        private readonly HookService hookService;
        private readonly PageNavigationService navigationService;

        public bool D2ProcStatus { get; set; }
        public Page CurrentPage { get; set; }

        public mainVM(HookService _hookService, PageNavigationService _navigationService)
        {
            this.hookService = _hookService;
            this.navigationService = _navigationService;
            _navigationService.OnPageChanged += _navigationService_OnPageChanged;
            //Scan();
        }

        private void _navigationService_OnPageChanged(Page page) => CurrentPage = page;

        //public Process Scan()
        //{
        //    var d2Proc = Process.GetProcessesByName("D2R");
        //    if (d2Proc.Length == 0) { D2ProcStatus = "Not Founed D2Process"; return null; }
        //    D2ProcStatus = $"OK! Name-{d2Proc[0].ProcessName}/Id-{d2Proc[0].Id}";
        //    return d2Proc[0];
        //}

        public ICommand StartLIstener => new DelegateCommand(() =>
        {
            var proc = Process.Start("notepad.exe");
            hookService.SetHook(proc);
        });
        public ICommand OpenKeyBindingPage => new DelegateCommand(() =>
        {
            navigationService.Navigate(new KeyBindingPage());
        });
       
    }
}
