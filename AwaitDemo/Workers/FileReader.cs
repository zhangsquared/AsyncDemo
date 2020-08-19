using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitDemo
{
    public class FileReader
    {
        private readonly string path;

        public FileReader(string path)
        {
            this.path = path;
        }

        public byte[] Read(int count)
        {
            Console.WriteLine($"Read start: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            var val = File.ReadAllBytes(path);
            Console.WriteLine($"Read stop: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return val;
        }

        public async Task<byte[]> ReadAsync(int count)
        {
            Console.WriteLine($"ReadAsync start: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            var rtn = await File.ReadAllBytesAsync(path);
            Console.WriteLine($"ReadAsync end: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return rtn;
        }

        public Task<byte[]> ReadTaskAsync(int count)
        {
            Console.WriteLine($"ReadTaskAsync start: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            var task = Task.Run(() =>
            {
                Console.WriteLine($"ReadTaskAsync (inside of new task): count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
                return File.ReadAllBytesAsync(path);
            });
            Console.WriteLine($"ReadTaskAsync end: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return task;
        }

    }
}
