namespace Mover
{
    using System;
    using System.Runtime.InteropServices;

    public class Win32
    {
        [DllImport("User32.dll")]
        public static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] InputWrapper[] inputWrapper, int structSize);

        public enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        public enum MouseEventFlags
        {
            Move = 0x00000001,
            LeftButtonDown = 0x00000002,
            LeftButtonUp = 0x00000004,
            RightButtonDown = 0x00000008,
            RightButtonUp = 0x00000010,
            MiddleButtonDown = 0x00000020,
            MiddleButtonUp = 0x00000040,
            Absolute = 0x00008000,
        }

        public struct MouseInput
        {
            public int dx;
            public int dy;
            public int mouseData;
            public MouseEventFlags dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        public struct KeyboardInput
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        public struct HardwareInput
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Input
        {
            [FieldOffset(0)]
            public MouseInput mouseInput;

            [FieldOffset(0)]
            public KeyboardInput keyboardInput;

            [FieldOffset(0)]
            public HardwareInput hardwareInput;
        }

        public struct InputWrapper
        {
            public InputType dwType;

            public Input Input;
        }
    }
}