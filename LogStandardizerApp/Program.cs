using System;
using System.IO;
using System.Text;

/* Задача 3
 * Консольная программа для стандартизации лог-файлов
 * Эта программа предназначена для обработки лог-файлов, содержащих записи в двух разных форматах.
 * Цель программы – привести все записи к единому, стандартному виду, упрощая анализ и обработку логов. 
 * Необходимо преобразовать записи из входного лог-файла в единый формат и сохранить их в выходной файл. 
 * 
 * УровеньЛогирования: может иметь одно из нескольких значений: 1)INFO (INFORMATION) 2)WARN (WARNING) 3)ERROR 4)DEBUG
 * Эти значения выбираются в зависимости от УровеньЛогирования входной записи. 
 */
namespace LogStandardizerApp
{
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                Console.WriteLine("Введите путь к входному лог-файлу:");
                string inputPath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputPath))
                {
                    Console.WriteLine("Ошибка: путь к файлу не указан.");
                    return;
                }

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine("Ошибка: указанный файл не найден.");
                    return;
                }

                string directory = Path.GetDirectoryName(inputPath) ?? Directory.GetCurrentDirectory();
                string outputPath = Path.Combine(directory, "standardized.txt");
                string problemsPath = Path.Combine(directory, "problems.txt");

                LogFileProcessor.ProcessFile(inputPath, outputPath, problemsPath);

                Console.WriteLine("Обработка завершена.");
                Console.WriteLine($"Стандартизированный файл: {outputPath}");
                Console.WriteLine($"Проблемные строки:        {problemsPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
            }
        }
    }    
}
