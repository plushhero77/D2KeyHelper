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

namespace D2KeyHelper.Services
{

    public class HookService : INotifyPropertyChanged
    {
        private readonly NativeWin32.NativeWin32.HookProc hookProc;
        private IntPtr hHook = IntPtr.Zero;
        private int gameProcessId;
        private readonly ProfileService profileService;

        public event PropertyChangedEventHandler PropertyChanged;

        public HookService(ProfileService _profileService)
        {
            profileService = _profileService;
            hookProc = new NativeWin32.NativeWin32.HookProc(HookCallback);
        }

        public bool IsHookSet { get; set; }

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            _ = NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);
            if (code < 0 || pid != gameProcessId) { return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam); }


            VirtualKeyShort wVk = Marshal.PtrToStructure<KEYBDINPUT>(lParam).wVk;
            src.BindingKey[] binding = profileService.CurrentProfile.BindingKeysCollection.Where(x => x.Value == wVk.ToString()).ToArray();

            if (binding.Length > 0 && ((Int32)Enum.Parse<WM_WPARAM>(binding[0].EventParam)) == wParam.ToInt32())
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