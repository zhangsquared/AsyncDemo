using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitDemo
{
    public class SimpleDemo
    {
        private readonly FakeJobs fake;

        public SimpleDemo(FakeJobs jobs)
        {
            fake = jobs;
        }

        public int Work()
        {
            int i = 0;
            fake.DoLongTimeJob1(ref i);
            fake.DoLongTimeJob2(ref i);
            fake.DoLongTimeJob3(ref i);
            return i;
        }

        public async Task<int> WorkParellelAsync()
        {
            int i = 0;
            Task t1 = Task.Run(() => fake.DoLongTimeJob1(ref i));
            Task t2 = Task.Run(() => fake.DoLongTimeJob2(ref i));
            Task t3 = Task.Run(() => fake.DoLongTimeJob3(ref i));
            await Task.WhenAll(t1, t2, t3);
            return i;
        }


        public async Task<int> WorkSerialAsync()
        {
            int i = 0;
            await Task.Run(() => fake.DoLongTimeJob1(ref i));
            await Task.Run(() => fake.DoLongTimeJob2(ref i));
            await Task.Run(() => fake.DoLongTimeJob3(ref i));
            return i;
        }

    }
}
