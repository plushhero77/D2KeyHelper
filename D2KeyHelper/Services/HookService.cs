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
using NativeWin32;

namespace D2KeyHelper.Services
{
    public class HookService
    {
        public event Action<IntPtr> OnKeyPressed;

        private NativeWin32.NativeWin32.HookProc hookProc = null;
        private IntPtr hHook;
        private int processId;
        private IntPtr hInstance;
        public NativeWin32Enums.WM_WPARAM WM_WPARAM { get; set; }

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
                if (wParam.ToInt32() == (int)WM_WPARAM)
                {
                    OnKeyPressed?.Invoke(lParam);
                }
            }

            return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
        public bool SetHook(Process process)
        {
            processId = process.Id;
            var hInstance = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(Process).Module);
            hHook = NativeWin32.NativeWin32.SetWindowsHookEx((int)eHookType.WH_KEYBOARD_LL, hookProc, hInstance, 0);
            if (hHook.ToInt32() == 0) { return false; }

            return true;
        }
        public bool DeleteHook() => NativeWin32.NativeWin32.UnhookWindowsHookEx(hHook) ? true : false;
    }
}

