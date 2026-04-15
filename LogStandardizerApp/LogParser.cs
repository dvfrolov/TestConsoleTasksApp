using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogStandardizerApp
{
    public static class LogParser
    {
        // 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
        private static readonly Regex Format1Regex = new Regex(
            @"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s+(?<level>[A-Z]+)\s+(?<message>.+)$",
            RegexOptions.Compiled);

        // 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'
        private static readonly Regex Format2Regex = new Regex(
            @"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\|\s*(?<level>[A-Z]+)\|(?<eventId>\d+)\|(?<method>[^|]+)\|\s*(?<message>.+)$",
            RegexOptions.Compiled);

        public static bool TryParse(string line, out LogEntry entry)
        {
            if (TryParseFormat1(line, out entry))
            {
                return true;
            }

            if (TryParseFormat2(line, out entry))
            {
                return true;
            }

            return false;
        }

        private static bool TryParseFormat1(string line, out LogEntry entry)
        {
            entry = null;

            Match match = Format1Regex.Match(line);
            if (!match.Success)
            {
                return false;
            }

            string dateText = match.Groups["date"].Value;
            string timeText = match.Groups["time"].Value;
            string levelText = match.Groups["level"].Value;
            string messageText = match.Groups["message"].Value;

            if (!DateTime.TryParseExact(
                    dateText,
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
            {
                return false;
            }

            if (!TryNormalizeLevel(levelText, out string normalizedLevel))
            {
                return false;
            }

            entry = new LogEntry(
                date,
                timeText,
                normalizedLevel,
                "DEFAULT",
                messageText);

            return true;
        }

        private static bool TryParseFormat2(string line, out LogEntry entry)
        {
            entry = null;

            Match match = Format2Regex.Match(line);
            if (!match.Success)
            {
                return false;
            }

            string dateText = match.Groups["date"].Value;
            string timeText = match.Groups["time"].Value;
            string levelText = match.Groups["level"].Value;
            string methodText = match.Groups["method"].Value.Trim();
            string messageText = match.Groups["message"].Value;

            if (!DateTime.TryParseExact(
                    dateText,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
            {
                return false;
            }

            if (!TryNormalizeLevel(levelText, out string normalizedLevel))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(methodText))
            {
                methodText = "DEFAULT";
            }

            entry = new LogEntry(
                date,
                timeText,
                normalizedLevel,
                methodText,
                messageText);

            return true;
        }

        private static bool TryNormalizeLevel(string sourceLevel, out string normalizedLevel)
        {
            normalizedLevel = string.Empty;

            switch (sourceLevel)
            {
                case "INFORMATION":
                case "INFO":
                    normalizedLevel = "INFO";
                    return true;

                case "WARNING":
                case "WARN":
                    normalizedLevel = "WARN";
                    return true;

                case "ERROR":
                    normalizedLevel = "ERROR";
                    return true;

                case "DEBUG":
                    normalizedLevel = "DEBUG";
                    return true;

                default:
                    return false;
            }
        }
    }
}
