using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AwaitDemo
{
    public class FakeJobs
    {
        public void DoLongTimeJob1(ref int i)
        {
            Thread.Sleep(3000);
            i += 5;
        }
        public void DoLongTimeJob2(ref int i)
        {
            Thread.Sleep(300);
            i *= 3;
        }
        public void DoLongTimeJob3(ref int i)
        {
            Thread.Sleep(2000);
            i += 2;
        }
    }
}
