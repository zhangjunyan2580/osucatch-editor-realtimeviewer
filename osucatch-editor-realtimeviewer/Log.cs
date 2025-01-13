using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osucatch_editor_realtimeviewer
{
    public static class Log
    {
        public enum LogType
        {
            Default,
            Program,
            EditorReader,
            BeatmapBuilder,
            BeatmapParser,
            Drawing,
            Backup,
            Timer,
        }

        public enum LogLevel { Debug, Info, Warning, Error }

        public static void ConsoleLog(string msg, LogType logType = LogType.Default, LogLevel logLevel = LogLevel.Info)
        {
            if (!app.Default.Show_Console) return;

            if (logType == LogType.Program && !app.Default.Log_Program) return;
            if (logType == LogType.EditorReader && !app.Default.Log_EditorReader) return;
            if (logType == LogType.BeatmapBuilder && !app.Default.Log_BeatmapBuilder) return;
            if (logType == LogType.BeatmapParser && !app.Default.Log_BeatmapParser) return;
            if (logType == LogType.Drawing && !app.Default.Log_Drawing) return;
            if (logType == LogType.Backup && !app.Default.Log_Backup) return;
            if (logType == LogType.Timer && !app.Default.Log_Timer) return;

            if (app.Default.Log_Level > (int)logLevel) return;

            Console.WriteLine("[" + logLevel + "] [" + DateTime.Now.ToString("HH:mm:ss.fff") + "] [" + logType + "] " + msg);
        }
    }
}
