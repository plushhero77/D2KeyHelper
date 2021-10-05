using D2KeyHelper.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NativeWin32.Enums;
using NativeWin32.Structs;

namespace D2KeyHelper.Services
{
    public class HookService
    {
        private NativeWin32.NativeWin32.HookProc hookProc;
        private IntPtr hHook;
        private int processId;
        public Dictionary<EBindedKeys, VirtualKeyShort> BindedKeys { get; }
        public WM_WPARAM WM_WPARAM { get; set; }

        public HookService()
        {
            this.hookProc = new NativeWin32.NativeWin32.HookProc(this.HookCallback);
            WM_WPARAM = WM_WPARAM.WM_KEYDOWN;
            BindedKeys = new Dictionary<EBindedKeys, VirtualKeyShort>();
            BindedKeys_Init();
        }

        private void BindedKeys_Init()
        {
            var keys = Enum.GetNames(typeof(EBindedKeys));

            for (int i = 0; i < keys.Length; i++)
            {
                BindedKeys.Add(Enum.Parse<EBindedKeys>(keys[i]), Enum.Parse<VirtualKeyShort>(Enum.GetName(typeof(VirtualKeyShort), 0x70 + i))); 
            }
        }



        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            if (code < 0) { return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam); }

            NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);

            if (pid == processId && ((int)WM_WPARAM) == wParam.ToInt32())
            {
                var pInputs = new[]
                {
                new INPUT()
                {
                    type = ((uint)INPUT_TYPE.INPUT_KEYBOARD),
                    U = new InputUnion()
                    {
                        ki = new KEYBDINPUT
                        {
                            wScan =  ScanCodeShort.KEY_Z,
                            wVk = VirtualKeyShort.KEY_Z
                        }
                    }
                }
            };
                NativeWin32.NativeWin32.SendInput(((uint)pInputs.Length), pInputs, INPUT.Size);
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
        public void SetKeyBinding (EBindedKeys key,VirtualKeyShort value)
        {
            BindedKeys[key] = value;
        }
    }

    public enum EBindedKeys
    {
        UserKey_1,
        UserKey_2,
        UserKey_3,
        UserKey_4,
        UserKey_5,
        UserKey_6,
        UserKey_7,
        UserKey_8,
        UserKey_9,
        UserKey_10,



    }
}

