using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogStandardizerApp
{
    public class LogEntry
    {
        public DateTime Date { get; }
        public string Time { get; }
        public string Level { get; }
        public string Method { get; }
        public string Message { get; }

        public LogEntry(DateTime date, string time, string level, string method, string message)
        {
            Date = date;
            Time = time;
            Level = level;
            Method = method;
            Message = message;
        }

        public string ToOutputString()
        {
            return string.Join(
                "\t",
                Date.ToString("yyyy-MM-dd"), // из "Примеры выходных данных"
                Time,
                Level,
                Method,
                Message);
        }
    }
}
