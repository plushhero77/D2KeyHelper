using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using NativeWin32.Enums;
using NativeWin32.Structs;

namespace D2KeyHelper.Services
{
    public class HookService
    {
        private readonly NativeWin32.NativeWin32.HookProc hookProc;

        private IntPtr hHook;
        private int processId;
        public bool IsHookSet => hHook.ToInt32() > 0;
        public ObservableCollection<BindingKey> BindingKeyCollection { get; }

        public HookService()
        {
            this.hookProc = new NativeWin32.NativeWin32.HookProc(this.HookCallback);
            BindingKeyCollection = new ObservableCollection<BindingKey>();
        }

        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);
            if (code < 0 || pid != processId) { return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam); }


            var wVk = Marshal.PtrToStructure<KEYBDINPUT>(lParam).wVk;
            var binding = BindingKeyCollection.Where(x => x.Value == wVk.ToString()).ToArray();

            if (binding.Length > 0 && ((Int32)Enum.Parse<WM_WPARAM>(binding[0].Event)) == wParam.ToInt32())
            {
                var pInputs = new[]
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

                NativeWin32.NativeWin32.SendInput(((uint)pInputs.Length), pInputs, INPUT.Size);
            }
            return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
        public bool SetHook(Process process)
        {
            processId = process.Id;
            var hInstance = Marshal.GetHINSTANCE(typeof(Process).Module);
            hHook = NativeWin32.NativeWin32.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, hookProc, hInstance, 0);
            return hHook.ToInt32() > 0;
        }
        public bool DeleteHook() => NativeWin32.NativeWin32.UnhookWindowsHookEx(hHook);

    }

    public class BindingKey
    {
        public BindingKey(string _key, VirtualKeyShort _value = VirtualKeyShort.F1, WM_WPARAM _event = WM_WPARAM.WM_KEYUP, bool _isMacros = false)
        {
            Key = _key;
            Value = _value.ToString();
            Event = _event.ToString();
            IsMarcos = _isMacros;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Event { get; set; }
        public bool IsMarcos { get; set; }
    }
}