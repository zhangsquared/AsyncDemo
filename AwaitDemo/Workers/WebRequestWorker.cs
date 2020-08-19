using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwaitDemo
{
    public class WebRequestWorker
    {
        private readonly WebRequest web;
        private readonly int loop = 5;

        public WebRequestWorker(WebRequest request)
        {
            web = request;
        }

        public void Work()
        {
            for (int i = 0; i < loop; i++)
            {
                web.Request(i);
            }
        }

        public async Task WorkAsync()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < loop; i++)
            {
                tasks.Add(web.RequestAsync(i));
            }
            await Task.WhenAll(tasks.ToArray());
        }

        public async Task WorkTaskAsync()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < loop; i++)
            {
                tasks.Add(web.RequestTaskAsync(i));
            }
            await Task.WhenAll(tasks.ToArray());
        }
    }
}
