using D2KeyHelper.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using static D2KeyHelper.Services.HookService;

namespace D2KeyHelper.Viewmodels.Main
{
    public class mainVM : BindableBase
    {
        private readonly HookService hookService;

        public string Key { get; set; } = "Key";
        public string RuningProcId { get; set; } = "RuningProcId";
        public string EventProcId { get; set; } = "EventProcId";
        public string D2ProcStatus { get; set; } = "Scan D2R Proces";

        public mainVM(HookService _hookService)
        {
            this.hookService = _hookService;
            hookService.KeyPressed += HookService_KeyPressed;
            Scan();
        }

        private void HookService_KeyPressed(IntPtr obj)
        {
            Key = typeof(ConsoleKey).GetEnumName(Marshal.ReadInt32(obj));

            NativeWin32.GetWindowThreadProcessId(NativeWin32.GetForegroundWindow(), out int id);
            EventProcId = id.ToString();

            const uint WM_LBUTTONDOWN = 0x0201;
            const uint WM_LBUTTONUP = 0x0202;


            //int X = X - WindowRect.left;
            //int Y = Y - winsowRect.top;
            //int lparm = (Y << 16) + X;
            //int lngResult = SendMessage(iHandle, WM_LBUTTONDOWN, 0, lparm);
            //int lngResult2 = SendMessage(iHandle, WM_LBUTTONUP, 0, lparm);

        }

        public Process Scan()
        {
            var d2Proc = Process.GetProcessesByName("D2R");
            if (d2Proc.Length == 0) { D2ProcStatus = "Not Founed D2Process"; return null; }
            D2ProcStatus = $"OK! Name-{d2Proc[0].ProcessName}/Id-{d2Proc[0].Id}";
            return d2Proc[0];
        }

        public ICommand StartLIstener => new DelegateCommand(() =>
        {

            var proc = Process.Start("notepad.exe");
            RuningProcId = proc.Id.ToString();
            hookService.SetHook(proc);
        });

    }
}
