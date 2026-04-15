using System;
using System.Threading;

namespace StaticClassServerApp
{
    /* Задача 2
     * Есть "сервер" в виде статического класса.
     * У него есть переменная count (тип int) и два метода, которые позволяют эту переменную читать и писать: GetCount() и AddToCount(int value).
     * К классу–"серверу" параллельно обращаются множество клиентов, которые в основном читают, но некоторые добавляют значение к count.
     * Нужно реализовать статический класс с методами GetCount / AddToCount так, чтобы:
     * 1) читатели могли читать параллельно, не блокируя друг друга;
     * 2) писатели писали только последовательно и никогда одновременно;
     * 3) пока писатели добавляют и пишут, читатели должны ждать окончания записи.
     */
    public static class Server // Демонстрация работы с классом Server вынесена в Main
    {
        private static int count = 0;

        // для параллельного чтения ReaderWriterLockSlim подходит лучше чем просто Lock
        private static readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public static int GetCount()
        {
            locker.EnterReadLock();
            try
            {
                return count;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public static void AddToCount(int value)
        {
            locker.EnterWriteLock();
            try
            {
                count += value;
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Начальное значение count: " + Server.GetCount());

                Thread[] readers = new Thread[5];
                Thread[] writers = new Thread[4];

                for (int i = 0; i < readers.Length; i++)
                {
                    int readerId = i + 1;
                    readers[i] = new Thread(() =>
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int value = Server.GetCount();
                            Console.WriteLine($"Читатель {readerId} прочитал count = {value}");
                            Thread.Sleep(100);
                        }
                    });
                }

                for (int i = 0; i < writers.Length; i++)
                {
                    int writerId = i + 1;
                    writers[i] = new Thread(() =>
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            Server.AddToCount(10);
                            Console.WriteLine($"Писатель {writerId} увеличил count на 10");
                            Thread.Sleep(150);
                        }
                    });
                }

                // стартовое значение 0, 4 писателя по 2 раза добавляют 10, итог должен быть 80
                foreach (Thread reader in readers)
                {
                    reader.Start();
                }

                foreach (Thread writer in writers)
                {
                    writer.Start();
                }

                foreach (Thread reader in readers)
                {
                    reader.Join();
                }

                foreach (Thread writer in writers)
                {
                    writer.Join();
                }
                Console.WriteLine("Итоговое значение count: " + Server.GetCount());
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally 
            { 
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
            }
        }
    }
}
