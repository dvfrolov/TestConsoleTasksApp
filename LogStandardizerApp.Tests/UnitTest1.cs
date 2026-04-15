using Xunit;

namespace LogStandardizerApp.Tests
{
    public class LogParserTests
    {
        [Fact]
        public void TryParse_Format1_ShouldParseCorrectly()
        {
            string line = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";

            bool result = LogParser.TryParse(line, out LogEntry? entry);

            Assert.True(result);
            Assert.NotNull(entry);
            Assert.Equal("2025-03-10", entry!.Date.ToString("yyyy-MM-dd"));
            Assert.Equal("15:14:49.523", entry.Time);
            Assert.Equal("INFO", entry.Level);
            Assert.Equal("DEFAULT", entry.Method);
            Assert.Equal("Версия программы: '3.4.0.48729'", entry.Message);
        }

        [Fact]
        public void TryParse_Format2_ShouldParseCorrectly()
        {
            string line = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'";

            bool result = LogParser.TryParse(line, out LogEntry? entry);

            Assert.True(result);
            Assert.NotNull(entry);
            Assert.Equal("2025-03-10", entry!.Date.ToString("yyyy-MM-dd"));
            Assert.Equal("15:14:51.5882", entry.Time);
            Assert.Equal("INFO", entry.Level);
            Assert.Equal("MobileComputer.GetDeviceId", entry.Method);
            Assert.Equal("Код устройства: '@MINDEO-M40-D-410244015546'", entry.Message);
        }

        [Fact]
        public void TryParse_UnknownLevel_ShouldReturnFalse()
        {
            string line = "10.03.2025 15:14:49.523 TRACE Тестовое сообщение";

            bool result = LogParser.TryParse(line, out LogEntry? entry);

            Assert.False(result);
            Assert.Null(entry);
        }

        [Fact]
        public void TryParse_InvalidLine_ShouldReturnFalse()
        {
            string line = "какой-то просто случайно зачем-то попавший сюда текст";

            bool result = LogParser.TryParse(line, out LogEntry? entry);

            Assert.False(result);
            Assert.Null(entry);
        }
    }
}