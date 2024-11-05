using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AudioToMicWPF.Services
{
    


    public class ClickSimulator
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        // 鼠标事件常量
        private const uint MOUSEEVENTF_LEFTDOWN = 0x00000002; // 左键按下
        private const uint MOUSEEVENTF_LEFTUP = 0x00000004;   // 左键抬起

        public static void HoldMouseLeftButton()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
        }

        public static void ReleaseMouseLeftButton()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
        }
    }
}