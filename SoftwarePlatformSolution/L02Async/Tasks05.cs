using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal class Tasks05
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"{nameof(RunMultipleAll)} sample TID:{Tid()}");
        }

        var demo = new Tasks05();
        demo.RunMultipleAll().Wait();
        Console.WriteLine();
    }

    private async Task RunMultipleAll()
    {
        int maxPrime = 60_000;
        int num, avg;
        Stopwatch sw = new();

        sw.Start();
        num = await NumOfPrimesAsync(maxPrime);
        avg = await AverageOfPrimesAsync(maxPrime);
        sw.Stop();

        Console.WriteLine($"Sequence - Num:{num} Avg:{avg} TID:{Tid()} Elapsed:{sw.Elapsed}");
        Console.WriteLine("---");

        sw.Restart();
        Task<int> numTask = NumOfPrimesAsync(maxPrime);
        Task<int> avgTask = AverageOfPrimesAsync(maxPrime);
        await Task.WhenAll(numTask, avgTask);
        num = numTask.Result;
        avg = avgTask.Result;
        sw.Stop();

        Console.WriteLine($"Parallel - Num:{num} Avg:{avg} TID:{Tid()} Elapsed:{sw.Elapsed}");
    }


    private Task<int> NumOfPrimesAsync(int maxPrime)
    {
        Task<int> computation = Task.Run(() =>
        {
            Console.WriteLine($"{nameof(NumOfPrimesAsync)} TID:{Tid()}");
            var primes = new Primes(maxPrime);
            return primes.Count();
        });

        return computation;
    }

    private Task<int> AverageOfPrimesAsync(int maxPrime)
    {
        Task<int> computation = Task.Run(() =>
        {
            Console.WriteLine($"{nameof(AverageOfPrimesAsync)} TID:{Tid()}");
            var primes = new Primes(maxPrime);
            return (int)primes.Average();
        });

        return computation;
    }


}
