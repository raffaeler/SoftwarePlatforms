using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal class Tasks02
{
    private const int MAX = 100_000;
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"Running the sample on TID:{Tid()}");
        }

        var demo = new Tasks02();

        demo.RunContinuation();
        Console.WriteLine();
    }

    private void RunContinuation()
    {
        Console.WriteLine("Starting asynchronous operations");

        var operationsTask = GetPrimesCountAsync(MAX)
            .ContinueWith((Task<int> t) => WriteResultAsync(t.Result));

        Console.WriteLine($"Asynchronous operation still ongoing!");
        while (!operationsTask.IsCompleted)
        {
            Console.Write(".");
            Thread.Sleep(300);
        }
        Console.WriteLine();
        Console.WriteLine($"Asynchronous execution finished: file written on disk");
    }

    private Task<int> GetPrimesCountAsync(int max)
    {
        // offload the computation in a different thread
        // the scheduler will take care of picking a thread
        Task<int> resTask = Task.Run(() =>
        {
            Console.WriteLine($"Running on TID:{Tid()}");

            var primes = new Primes(max);
            var res = primes.Count();
            return res;
        });

        return resTask;
    }

    private Task WriteResultAsync(int number)
    {
        Console.WriteLine();
        Console.WriteLine($"WriteResultAsync> writing on disk the result");

        var content = $"The number of primesunder {MAX} is {number}";

        var fileTask = File.WriteAllTextAsync("Tasks02_Result.txt", content);

        return fileTask;
    }





}
