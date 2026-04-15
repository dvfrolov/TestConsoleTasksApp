using System.IO;
using System.Text;

namespace LogStandardizerApp
{
    public static class LogFileProcessor
    {
        public static void ProcessFile(string inputPath, string outputPath, string problemsPath)
        {
            using (var reader = new StreamReader(inputPath, Encoding.UTF8))
            using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
            using (var problemsWriter = new StreamWriter(problemsPath, false, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        problemsWriter.WriteLine(line);
                    }
                    else if(LogParser.TryParse(line, out LogEntry entry))
                    {
                        writer.WriteLine(entry.ToOutputString());
                    }
                    else
                    {
                        problemsWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}
