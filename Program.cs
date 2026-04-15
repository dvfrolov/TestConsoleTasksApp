using System;
using System.Text;

/* Задача 1
 *  Дана строка, содержащая n маленьких букв латинского алфавита. 
 *  Требуется реализовать алгоритм компрессии этой строки, замещающий группы последовательно идущих одинаковых букв формой "sc" 
 *  где "s" – символ, "с" – количество букв в группе, а также алгоритм декомпрессии, возвращающий исходную строку по сжатой.
 *  Если буква в группе всего одна – количество в сжатой строке не указываем, а пишем её как есть. 
 * Пример: 
 *  Исходная строка: aaabbcccdde 
 *  Сжатая строка: a3b2c3d2e
 */
namespace StringCompressionApp
{
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                Console.WriteLine("Введите строку из маленьких латинских букв:");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: строка не должна быть пустой");
                    return;
                }

                string compressed = StringCompressor.Compress(input);
                string decompressed = StringCompressor.Decompress(compressed);

                Console.WriteLine($"Исходная строка: {input}");
                Console.WriteLine($"Сжатая строка:   {compressed}");
                Console.WriteLine($"Восстановлено:   {decompressed}");
                Console.WriteLine($"Корректно:       {string.Equals(input, decompressed, StringComparison.Ordinal)}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка данных: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
            }
        }
    }

    public static class StringCompressor
    {
        public static string Compress(string input)
        {
            ValidateSourceString(input);

            if (input.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            char currentChar = input[0];
            int count = 1;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == currentChar)
                {
                    count++;
                }
                else
                {
                    AppendCompressedGroup(result, currentChar, count);
                    currentChar = input[i];
                    count = 1;
                }
            }

            AppendCompressedGroup(result, currentChar, count);

            return result.ToString();
        }

        public static string Decompress(string compressed)
        {
            ValidateCompressedString(compressed);

            if (compressed.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            int i = 0;

            while (i < compressed.Length)
            {
                char currentChar = compressed[i];

                if (!IsLowerLatinLetter(currentChar))
                {
                    throw new ArgumentException("Сжатая строка должна содержать только маленькие латинские буквы и числа после них.");
                }

                i++;

                int count = 0;
                bool hasDigits = false;

                while (i < compressed.Length && char.IsDigit(compressed[i]))
                {
                    hasDigits = true;
                    count = count * 10 + (compressed[i] - '0');
                    i++;
                }

                if (!hasDigits)
                {
                    count = 1;
                }
                else if (count <= 1)
                {
                    throw new ArgumentException("Некорректная сжатая строка: количество в группе должно быть больше 1.");
                }

                result.Append(currentChar, count);
            }

            return result.ToString();
        }

        private static void AppendCompressedGroup(StringBuilder builder, char symbol, int count)
        {
            builder.Append(symbol); // StringBuilder, тк. лучше многократной конкатенации строк

            if (count > 1)
            {
                builder.Append(count);
            }
        }

        private static void ValidateSourceString(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            foreach (char c in input)
            {
                if (!IsLowerLatinLetter(c))
                {
                    throw new ArgumentException("Исходная строка должна содержать только маленькие буквы причем только из латинского алфавита.");
                }
            }
        }

        private static void ValidateCompressedString(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Length == 0)
            {
                return;
            }

            if (!IsLowerLatinLetter(input[0]))
            {
                throw new ArgumentException("Сжатая строка должна начинаться с маленькой и при этом только латинской буквы.");
            }
        }

        private static bool IsLowerLatinLetter(char c)
        {
            return c >= 'a' && c <= 'z';
        }
    }
}
