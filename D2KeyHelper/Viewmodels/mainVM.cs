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
        public string D2ExeFilePath { get; set; }
        public Process D2RProcess { get; set; }

        public MainVM()
        {
            Initialize();
            StartProcWather();
        }

        public DelegateCommand OpenFileDialog => new DelegateCommand(() =>
        {
            OpenFileDialog fdiag = new();
            fdiag.Filter = "Executable File (*.exe)|*.exe";
            fdiag.InitialDirectory = Directory.GetCurrentDirectory();

            if (fdiag.ShowDialog() == true)
            {
                D2ExeFilePath = fdiag.FileName;
            }
        });
        public DelegateCommand RunD2Process => new DelegateCommand(() =>
        {
            Process.Start(D2ExeFilePath);
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
