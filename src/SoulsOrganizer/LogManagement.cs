using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SoulsOrganizer
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

    public class Log
    {
        public DateTime Date { get; private set; }
        public LogType Type { get; private set; }
        public string Message { get; private set; }

        public Log(DateTime date, LogType type, string message)
        {
            Date = date;
            Type = type;
            Message = message;
        }

        public override string ToString()
        {
            return string.Format("{0:HH:mm:ss} - {1}", Date, Message);
        }

    }

    public enum LogType
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
}
