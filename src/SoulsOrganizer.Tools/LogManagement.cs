using System;
using System.Collections.ObjectModel;

namespace SoulsOrganizer.Tools
{
    public static class LogManagement
    {
        public static ObservableCollection<Log> Logs { get; private set; }
        public static Log Last { get; private set; }

        static LogManagement()
        {
            Logs = new ObservableCollection<Log> ();
        }

        public static void AddInfo(string message)
        {
            Add(new Log(DateTime.Now, LogType.Info, message));
        }
        
        public static void AddWarning(string message)
        {
            Add(new Log(DateTime.Now, LogType.Warning, message));
        }

        public static void AddError(string message)
        {
            Add(new Log(DateTime.Now, LogType.Error, message));
        }

        private static void Add(Log log)
        {
            Logs.Insert(0, log);
            Last = log;
        }

    }
}
