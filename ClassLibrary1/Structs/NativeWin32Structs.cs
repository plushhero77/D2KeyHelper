using NativeWin32.Enums;
using System;
using System.Runtime.InteropServices;

namespace NativeWin32.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public uint type;
        public InputUnion U;
        public static int Size
        {
            get { return Marshal.SizeOf(typeof(INPUT)); }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        internal int dx;
        internal int dy;
        internal int mouseData;
        internal MOUSEEVENTF dwFlags;
        internal uint time;
        internal UIntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public VirtualKeyShort wVk;
        public ScanCodeShort wScan;
        internal KEYEVENTF dwFlags;
        internal int time;
        internal UIntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        internal int uMsg;
        internal short wParamL;
        internal short wParamH;
    }
}
