using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitDemo
{
    public class WebRequest
    {
        private readonly string url;

        public WebRequest(string url)
        {
            this.url = url;
        }

        public HttpResponseMessage Request(int count)
        {
            Console.WriteLine($"Request start: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");

            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            HttpClient client =  HttpClientFactory.Create();
            HttpResponseMessage message = client.GetAsync(url).Result;

            Console.WriteLine($"Request end: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return message;
        }

        public async Task<HttpResponseMessage> RequestAsync(int count)
        {
            Console.WriteLine($"RequestAsync start: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");

            HttpClient client = HttpClientFactory.Create();
            HttpResponseMessage message = await client.GetAsync(url);

            Console.WriteLine($"RequestAsync end: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return message;
        }

        public Task<HttpResponseMessage> RequestTaskAsync(int count)
        {
            Console.WriteLine($"RequestTaskAsync start: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");

            var task = Task.Run(() =>
            {
                Console.WriteLine($"RequestTaskAsync (inside of new task): count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");

                HttpClient client = HttpClientFactory.Create();
                return client.GetAsync(url);
            });
            Console.WriteLine($"RequestTaskAsync end: count: {count}; Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            return task;
        }
    }
}
