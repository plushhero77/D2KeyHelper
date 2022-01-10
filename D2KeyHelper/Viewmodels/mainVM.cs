﻿using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Management;
using System.Threading;
using System;
using System.Configuration;
using System.Windows;
using D2KeyHelper.src;
using D2KeyHelper.src.Interfaces;

namespace D2KeyHelper.Viewmodels
{
    public class MainVM : BindableBase
    {
        public Process D2RProcess { get; set; }
        public HookService HookService { get; }
        public SettingsService SettingsService { get; }
        public ProfileService ProfileService { get; }

        private readonly IWindowManagmentService _windowService;

        public MainVM(HookService hookService, SettingsService settingsService, ProfileService profileService, IWindowManagmentService windowService)
        {
            Initialize();
            StartProcWather();
            HookService = hookService;
            SettingsService = settingsService;
            ProfileService = profileService;
            _windowService = windowService;
        }

        public DelegateCommand OpenFileDialog => new(() =>
        {
            OpenFileDialog fdiag = new();
            fdiag.Filter = "Executable File (*.exe)|*.exe";
            fdiag.InitialDirectory = Directory.GetCurrentDirectory();

            if (fdiag.ShowDialog() == true)
            {
                SettingsService.Settings.ExeFilePath = fdiag.FileName;
            }
        });
        public DelegateCommand RunD2Process => new(() =>
           {
               Process.Start(SettingsService.Settings.ExeFilePath);
           });
        public DelegateCommand AddProfile => new(() =>
        {
            _ = _windowService.ShowModalViewModel<EditProfileVM>();
        });
        public DelegateCommand EditProfile => new(() =>
        {
            _ = _windowService.ShowModalViewModel<EditProfileVM>();
        });
        public DelegateCommand DeleteProfile => new(() => { MessageBox.Show(ProfileService.CurrentProfile.ToString()); });
        public DelegateCommand SetHook => new(() => {HookService.SetHook(D2RProcess.Id); },!HookService.IsHookSet && D2RProcess !=null);
        public DelegateCommand UnsetHook => new(() => { HookService.DeleteHook(); },HookService.IsHookSet);


        private void Initialize()
        {
            var processses = Process.GetProcessesByName("d2r");
            D2RProcess = processses.Length > 0 ? processses[0] : null;
        }
        private void StartProcWather()
        {
            var procDeletingWatcher = new ManagementEventWatcher(new WqlEventQuery("__InstanceDeletionEvent", new TimeSpan(0, 0, 0, 1, 0), "TargetInstance isa \"Win32_Process\""));
            var procCreatingWather = new ManagementEventWatcher(new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 0, 1, 0), "TargetInstance isa \"Win32_Process\""));

            procCreatingWather.EventArrived += new EventArrivedEventHandler(delegate (object sender, EventArrivedEventArgs e)
           {
               if (((ManagementBaseObject)e.NewEvent["TargetInstance"])["Name"].ToString().ToLower().Contains("d2r"))
               {

                   D2RProcess = Process.GetProcessesByName("d2r")[0];
               }
           });
            procCreatingWather.Scope.Options.EnablePrivileges = true;
            procCreatingWather.Start();

            procDeletingWatcher.EventArrived += new EventArrivedEventHandler(delegate (object sender, EventArrivedEventArgs e)
            {
                if (((ManagementBaseObject)e.NewEvent["TargetInstance"])["Name"].ToString().ToLower().Contains("notepad"))
                {

                    D2RProcess = null;
                }
            });
            procDeletingWatcher.Scope.Options.EnablePrivileges = true;
            procDeletingWatcher.Start();
        }

    }
}
