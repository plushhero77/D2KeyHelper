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
using D2KeyHelper.src.Interfaces;

namespace D2KeyHelper.Services
{

    public class HookService : BindableBase, IHookService
    {
        private readonly NativeWin32.NativeWin32.HookProc hookProc;
        private readonly IProfileService _profileService;
        private readonly ISettingsService _settingsService;
        private IntPtr hHook = IntPtr.Zero;
        private int gameProcessId;


        public HookService(IProfileService profileService, ISettingsService settingsService)
        {
            hookProc = new NativeWin32.NativeWin32.HookProc(HookCallback);
            _profileService = profileService;
            _settingsService = settingsService;
        }

        public bool IsHookSet { get; private set; }

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            _ = NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);
            if (code < 0 || pid != gameProcessId)  
                return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam); 


            VirtualKeyShort wVk = Marshal.PtrToStructure<KEYBDINPUT>(lParam).wVk;
            var binding = _profileService.CurrentProfile?.KeyBindingCollection.Where(x => x.KeyShort == wVk).ToArray();
            if (binding == null)
                return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);

            WM_WPARAM keyEvent = _settingsService.Settings.IsKeyUpEvent ? WM_WPARAM.WM_KEYUP : WM_WPARAM.WM_KEYDOWN;

            if (binding.Length > 0 && (Int32)keyEvent == wParam.ToInt32())
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
                IsHookSet = !NativeWin32.NativeWin32.UnhookWindowsHookEx(hHook);
            }
            return IsHookSet;
        }

    }

}