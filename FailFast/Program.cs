using System;
using System.Threading.Tasks;

namespace FailFast
{
    class Program
    {
        static void Main(string[] args)
        {
            Test t = new Test();

            Task.Run(() => t.Start());
            Console.ReadLine();

            Task.Run(() => t.Start2());
            Console.ReadLine();

            Task.Run(() => t.Start2Async());
            Console.ReadLine();

            Task.Run(() => t.Start3Async());
            Console.ReadLine();
        }

    }
}
