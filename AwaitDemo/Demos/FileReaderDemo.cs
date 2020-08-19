using System;
using System.Diagnostics;

namespace AwaitDemo
{
    public static class FileReaderDemo
    {
        public static void Run(string file = null)
        {
            string path = file ?? @"C:\Users\zzhang\Downloads\big.txt";

            FileReader reader = new FileReader(path);
            FileReaderWorker worker = new FileReaderWorker(reader);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            worker.Work();
            stopwatch.Stop();
            Console.WriteLine($"Work: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            ConsoleLine();

            stopwatch.Start();
            worker.WorkAsync().Wait();
            stopwatch.Stop();
            Console.WriteLine($"WorkAsync: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            ConsoleLine();

            stopwatch.Start();
            worker.WorkTaskAsync().Wait();
            stopwatch.Stop();
            Console.WriteLine($"WorkTaskAsync: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();
        }

        private static void ConsoleLine()
        {
            Console.WriteLine("---------------------------------");
        }
    }
}
