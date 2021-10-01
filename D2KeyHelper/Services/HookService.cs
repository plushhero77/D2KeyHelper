using D2KeyHelper.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace D2KeyHelper.Services
{
    public class HookService
    {
        public event Action<IntPtr> KeyPressed;

        private NativeWin32.NativeWin32.HookProc hookProc = null;
        private IntPtr hHook;
        private int processId;
        public HookService()
        {
            this.hookProc = new NativeWin32.NativeWin32.HookProc(this.HookCallback);
        }

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            if (code < 0)
            {
                return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }

            NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);

            if (pid == processId)
            {
                KeyPressed?.Invoke(lParam);
            }

            return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
        public void SetHook(Process process)
        {
            processId = process.Id;
            var hInstance = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(Process).Module);
            hHook = NativeWin32.NativeWin32.SetWindowsHookEx((int)eHookType.WH_KEYBOARD_LL, hookProc, hInstance, 0);
            if ((int)hHook == 0)
            {
                var sys = Marshal.GetLastWin32Error();
                throw new Exception($"Error Code - {sys}");
            }
        }
        
    }
}

