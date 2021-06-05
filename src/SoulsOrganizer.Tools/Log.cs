using System;

namespace SoulsOrganizer.Tools
{
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
}
