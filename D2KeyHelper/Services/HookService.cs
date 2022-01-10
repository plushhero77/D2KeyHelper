using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using NativeWin32.Structs;
using NativeWin32.Enums;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using PropertyChanged;
using DevExpress.Mvvm;

namespace D2KeyHelper.Services
{

    public class HookService : BindableBase
    {
        private readonly NativeWin32.NativeWin32.HookProc hookProc;
        private readonly ProfileService _profileService;
        private IntPtr hHook = IntPtr.Zero;
        private int gameProcessId;


        public HookService(ProfileService profileService)
        {
            hookProc = new NativeWin32.NativeWin32.HookProc(HookCallback);
            _profileService = profileService;
        }

        public bool IsHookSet { get; set; }

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            _ = NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);
            if (code < 0 || pid != gameProcessId) { return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam); }


            VirtualKeyShort wVk = Marshal.PtrToStructure<KEYBDINPUT>(lParam).wVk;
            var binding = _profileService.CurrentProfile.KeyBindingCollection.Where(x => x.KeyShort == wVk).ToArray();

            if (binding.Length > 0 && (Int32)WM_WPARAM.WM_KEYDOWN == wParam.ToInt32())
            {
                INPUT[] pInputs = new[]
                {
                        new INPUT()
                        {
                            type  =(uint)INPUT_TYPE.INPUT_MOUSE,
                            U =new InputUnion()
                            {
                                mi= new MOUSEINPUT
                                {
                                    dwFlags = MOUSEEVENTF.RIGHTDOWN
                                }
                            }

                        },
                        new INPUT()
                        {
                            type  =(uint)INPUT_TYPE.INPUT_MOUSE,
                            U =new InputUnion()
                            {
                                mi= new MOUSEINPUT
                                {
                                    dwFlags = MOUSEEVENTF.RIGHTUP
                                }
                            }
                        }
                 };

                _ = NativeWin32.NativeWin32.SendInput(((uint)pInputs.Length), pInputs, INPUT.Size);
            }
            return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
        public bool SetHook(int id)
        {
            if (!IsHookSet)
            {
                gameProcessId = id;
                IntPtr hInstance = Marshal.GetHINSTANCE(typeof(Process).Module);
                hHook = NativeWin32.NativeWin32.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, hookProc, hInstance, 0);
            }
            return IsHookSet = hHook.ToInt32() > 0;
        }
        public bool DeleteHook()
        {
            if (IsHookSet)
            {
                _ = !NativeWin32.NativeWin32.UnhookWindowsHookEx(hHook);
            }
            return IsHookSet = hHook.ToInt32() > 0;
        }

    }

}