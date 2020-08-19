using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsAsync
{
    public class FakeWork
    {
        public event EventHandler ProgressEvent;
        public async Task LongRunAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine($"LongRunAsync Thread ID = {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(500);
                    i++;
                    ProgressEvent?.Invoke(i, EventArgs.Empty);
                });
            }
        }


        public int Calc(int i)
        {
            Console.WriteLine($"Calc Thread ID = {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(500);
            i++;
            return i;
        }

        public async Task<int> CalcAsync(int i)
        {
            Console.WriteLine($"before CalcAsync Thread ID = {Thread.CurrentThread.ManagedThreadId}");

            await Task.Run(() =>
            {
                Console.WriteLine($"Sleep Thread ID = {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(500);
                i++;
            });

            Console.WriteLine($"after CalcAsync Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return i;
        }

    }
}
