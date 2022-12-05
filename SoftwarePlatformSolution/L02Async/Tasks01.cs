using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal class Tasks01
{
    private const int MAX = 100_000;
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"Running the sample on TID:{Tid()}");
        }

        var demo = new Tasks01();

        Console.WriteLine("Starting synchronous execution");
        int result = demo.GetPrimesCount(MAX);
        Console.WriteLine($"Synchronous execution finished: {result}");
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Starting asynchronous execution");
        Task<int> resultTask = demo.GetPrimesCountAsync(MAX);
        Console.WriteLine($"Asynchronous operation still ongoing!");
        while(!resultTask.IsCompleted)
        {
            Console.Write(".");
            Thread.Sleep(300);
        }
        Console.WriteLine();
        var result2 = resultTask.Result;
        Console.WriteLine($"Asynchronous execution finished: {result2}");
    }

    private int GetPrimesCount(int max)
    {
        Console.WriteLine($"Running on TID:{Tid()}");

        var primes = new Primes(max);
        var res = primes.Count();
        return res;
    }

    private Task<int> GetPrimesCountAsync(int max)
    {
        // offload the computation in a different thread
        // the scheduler will take care of picking a thread
        //Task<int> resTask = Task.Run(() =>
        //{
        //    Console.WriteLine($"Running on TID:{Tid()}");

        //    var primes = new Primes(max);
        //    var res = primes.Count();
        //    return res;
        //});

        Task<int> resTask = Task.Run(() => DoOperation(max));

        return resTask;
    }

    private int DoOperation(int max)
    {
        Console.WriteLine($"Running on TID:{Tid()}");

        var primes = new Primes(max);
        var res = primes.Count();
        return res;
    }

}
