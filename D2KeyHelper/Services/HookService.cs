using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using NativeWin32.Enums;
using NativeWin32.Structs;

namespace D2KeyHelper.Services
{
    public class HookService
    {
        private NativeWin32.NativeWin32.HookProc hookProc;
        private IntPtr hHook;
        private int processId;
        public ObservableCollection<BindingKey> BindingKeyCollection { get; }
        public HookService()
        {
            this.hookProc = new NativeWin32.NativeWin32.HookProc(this.HookCallback);
            BindingKeyCollection = new ObservableCollection<BindingKey>();
            BindedKeys_Init();
        }

        private void BindedKeys_Init() // Заменить на Профиль
        {
            BindingKeyCollection.Add(new BindingKey("key", VirtualKeyShort.F3, WM_WPARAM.WM_KEYUP));
        }



        private int HookCallback(int code, IntPtr wParam, IntPtr lParam)
        {

            if (code < 0) { return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam); }

           NativeWin32.NativeWin32.GetWindowThreadProcessId(NativeWin32.NativeWin32.GetForegroundWindow(), out int pid);

            if (pid == processId)
            {
                var wVk = Marshal.PtrToStructure<KEYBDINPUT>(lParam).wVk;
                var binding = BindingKeyCollection.Where(x => x.Value == wVk).ToArray();

                if (binding.Length > 0 && ((int)binding[0].Event) == wParam.ToInt32())
                {
                    var pInputs = new[]
                    {
                        new INPUT()
                        {
                            type = (uint)INPUT_TYPE.INPUT_KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT
                                {
                                    wScan = ScanCodeShort.KEY_Z,
                                    wVk = VirtualKeyShort.KEY_Z
                                }
                            }
                        }                   
                    };
                    //Thread.Sleep(400);
                    NativeWin32.NativeWin32.SendInput(((uint)pInputs.Length), pInputs, INPUT.Size);
                }
            }




            return NativeWin32.NativeWin32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        public bool SetHook(Process process)
        {
            processId = process.Id;
            var hInstance = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(Process).Module);
            hHook = NativeWin32.NativeWin32.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, hookProc, hInstance, 0);
            if (hHook.ToInt32() == 0) { return false; }

            return true;
        }
        public bool DeleteHook() => NativeWin32.NativeWin32.UnhookWindowsHookEx(hHook) ? true : false;
        public void EditKeyBinding(string key, VirtualKeyShort value)
        {
            BindingKeyCollection.FirstOrDefault(x => x.Key == key).Value = value;
        }

        public void AddKeyBinding(string key, VirtualKeyShort value)
        {
            BindingKeyCollection.Add(new BindingKey(key, VirtualKeyShort.KEY_Z, WM_WPARAM.WM_KEYDOWN));
        }
        public void DeleteKeyBinding(string key)
        {
            BindingKeyCollection.Remove(BindingKeyCollection.FirstOrDefault(x => x.Key == key));
        }

    }
    public class BindingKey
    {
        public BindingKey(string _key, VirtualKeyShort _value, WM_WPARAM _event = WM_WPARAM.WM_KEYUP)
        {
            Key = _key;
            Value = _value;
            Event = _event;
        }

        public string Key { get; set; }
        public VirtualKeyShort Value { get; set; }
        public WM_WPARAM Event { get; set; }
    }
}