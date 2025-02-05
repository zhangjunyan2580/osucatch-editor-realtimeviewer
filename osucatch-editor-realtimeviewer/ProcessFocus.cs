using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace osucatch_editor_realtimeviewer
{
    public static class ProcessFocus
    {
        // Windows API：获取当前焦点窗口的句柄
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        // Windows API：获取窗口的进程 ID
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public static bool IsEditorForeground()
        {
            // 获取当前焦点窗口的句柄
            IntPtr foregroundWindow = GetForegroundWindow();

            // 获取焦点窗口的进程 ID
            if (GetWindowThreadProcessId(foregroundWindow, out uint processId) > 0)
            {
                // 获取进程
                Process process = Process.GetProcessById((int)processId);

                // 检查是否是目标进程
                if (process.MainModule != null && process.MainModule.ModuleName == "osu!.exe" && process.MainModule.FileVersionInfo.ProductName == "osu!")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
