using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AwaitDemo
{
    public static class WebRequestDemo
    {
        public static void Run(string link = null)
        {
            string url = link ?? "https://localhost:5001";

            WebRequest request = new WebRequest(url);
            WebRequestWorker worker = new WebRequestWorker(request);

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
