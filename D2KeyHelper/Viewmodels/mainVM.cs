using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Management;
using System.Threading;
using System;
using System.Configuration;

namespace D2KeyHelper.Viewmodels
{
    public class MainVM : BindableBase
    {
        public Process D2RProcess { get; set; }
        public SettingsService SettingsService { get; }
        public ProfileService ProfileService { get; }
        public string[] ProfilesNames { get; set; }

        public MainVM(SettingsService settingsService,ProfileService profileService)
        {
            Initialize();
            StartProcWather();
            SettingsService = settingsService;
            ProfileService = profileService;
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
        public DelegateCommand test => new(() =>
        {

        });
        public DelegateCommand AddProfile => new(() =>
        {
            ProfileService.Add(new src.Profile());
        });


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
