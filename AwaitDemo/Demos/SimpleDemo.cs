using System;
using System.Diagnostics;

namespace AwaitDemo
{
    public static class SimpleDemo
    {
        public static void Run()
        {
            FakeJobs jobs = new FakeJobs();
            FakeJobWorker worker = new FakeJobWorker(jobs);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            int val1 = worker.Work();
            stopwatch.Stop();
            Console.WriteLine($"Value: {val1}; Sync: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            stopwatch.Start();
            int val2 = worker.WorkParellelAsync().Result;
            stopwatch.Stop();
            Console.WriteLine($"Value: {val2}; Parellel: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            stopwatch.Start();
            int val3 = worker.WorkSerialAsync().Result;
            stopwatch.Stop();
            Console.WriteLine($"Value: {val3}; Serial: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();
        }
    }
}
