using System;
using System.Diagnostics;

namespace AwaitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            FakeJobs jobs = new FakeJobs();
            SimpleDemo demo = new SimpleDemo(jobs);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            int val1 = demo.Work();
            stopwatch.Stop();
            Console.WriteLine($"Value: {val1}; Sync: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            stopwatch.Start();
            int val2 = demo.WorkParellelAsync().Result;
            stopwatch.Stop();
            Console.WriteLine($"Value: {val2}; Parellel: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            stopwatch.Start();
            int val3 = demo.WorkSerialAsync().Result;
            stopwatch.Stop();
            Console.WriteLine($"Value: {val3}; Serial: {stopwatch.ElapsedMilliseconds} Milliseconds");
            stopwatch.Reset();

            Console.WriteLine();
            Console.WriteLine("-END-");
            Console.ReadKey();
        }
    }
}
