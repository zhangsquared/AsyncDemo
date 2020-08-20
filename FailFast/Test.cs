using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FailFast
{
    public class Test
    {
        /// <summary>
        /// Mock method to mimic a long-time job
        /// </summary>
        /// <param name="waitTimeInSec">mimic the wait time of the job</param>
        /// <param name="finalResult">mimic the return value of after job finished</param>
        public bool DoJob(string jobName, int waitTimeInSec, bool finalResult)
        {
            Console.WriteLine($"{jobName} has started...");
            Thread.Sleep(TimeSpan.FromSeconds(waitTimeInSec));
            Console.WriteLine($"{jobName} has finished with return = {finalResult}");
            return finalResult;
        }

        /// <summary>
        /// this is a method that wait until every task is done
        /// then return the result
        /// apparently this is not efficient
        /// </summary>
        public async Task<bool> Start()
        {
            Task<bool> a = Task.Run(() => DoJob("a", 10, true));
            Task<bool> b = Task.Run(() => DoJob("b", 8, true));
            Task<bool> c = Task.Run(() => DoJob("c", 1, false));
            bool[] results = await Task.WhenAll(a, b, c);
            bool final = results.All(x => x);

            Console.WriteLine($"-- final result: {final}");
            return final;
        }

        /// <summary>
        /// this will return fail much faster
        /// if any task has failed, immediately return false
        /// the problem is that job a and job b will contiue consume resources
        /// i want to cancel job a and job b if job c has failed
        /// </summary>
        public bool Start2()
        {
            bool failed = false;
            Task a = Task.Run(() => DoJob("a", 10, true)).ContinueWith((t) => 
            {
                bool res = t.Result;
                if (!res)
                {
                    failed = true;
                    Console.WriteLine($"-- final result: {!failed}");
                }
            });
            Task b = Task.Run(() => DoJob("b", 8, true)).ContinueWith((t) =>
            {
                bool res = t.Result;
                if (!res)
                {
                    failed = true;
                    Console.WriteLine($"-- final result: {!failed}");
                }
            });
            Task c = Task.Run(() => DoJob("c", 1, false)).ContinueWith((t) =>
            {
                bool res = t.Result;
                if (!res)
                {
                    failed = true;
                    Console.WriteLine($"-- final result: {!failed}");
                }
            });
            return !failed;
        }

        /// <summary>
        /// same idea with Start2, different implementation
        /// </summary>
        public async Task<bool> Start2Async()
        {
            Task<bool> a = Task.Run(() => DoJob("a", 10, true));
            Task<bool> b = Task.Run(() => DoJob("b", 8, true));
            Task<bool> c = Task.Run(() => DoJob("c", 1, false));
            List<Task<bool>> tasks = new List<Task<bool>> { a, b, c };
            while (tasks.Any())
            {
                Task<bool> completedTask = await Task.WhenAny(tasks);
                if (!await completedTask)
                {
                    Console.WriteLine($"-- final result: {false}");
                    return false;
                }
                tasks.Remove(completedTask);
            }
            return true;
        }

        /// <summary>
        /// Modified version of cancellable task
        /// Pass the token to the cancelable operation.
        /// </summary>
        public bool? DoJob(string jobName, int waitTimeInSec, bool finalResult, CancellationToken token, int loop)
        {
            // Was cancellation already requested?
            if (token.IsCancellationRequested)
            {
                Console.WriteLine($"{jobName} was cancelled before it got started.");
                //token.ThrowIfCancellationRequested();
                return null;
            }

            Console.WriteLine($"{jobName} has started...");
            TimeSpan ts = TimeSpan.FromSeconds(waitTimeInSec);
            TimeSpan piece = ts / loop;
            for (int i = 0; i <= loop; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"{jobName} cancelled");
                    //token.ThrowIfCancellationRequested();
                    return null;
                }

                Thread.Sleep(piece);
            }
            Console.WriteLine($"{jobName} has finished with return = {finalResult}");
            return finalResult;
        }

        /// <summary>
        /// fail fast: if any task fail, immediately return false, and cancel the rest of remaining job
        /// </summary>
        public async Task<bool> Start3Async()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            int iterationNum = 100;
            Task<bool?> a = Task.Run(() => DoJob("a", 10, true, token, iterationNum));
            Task<bool?> b = Task.Run(() => DoJob("b", 8, true, token, iterationNum));
            Task<bool?> c = Task.Run(() => DoJob("c", 1, false, token, iterationNum));
            List<Task<bool?>> tasks = new List<Task<bool?>> { a, b, c };
            while (tasks.Any())
            {
                Task<bool?> completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);
                bool? res = await completedTask;
                if (res.HasValue && !res.Value)
                {
                    source.Cancel();
                    Console.WriteLine($"-- final result: {false}");
                    return false;
                }
            }
            return true;
        }


    }
}
