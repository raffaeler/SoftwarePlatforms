using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

// Using WhenAny to continue the execution as soon as the **first**
// asynchronous operation is concluded successfully

namespace L02Async;

internal class Tasks06
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"{nameof(RunMultipleAny)} sample TID:{Tid()}");
        }

        var demo = new Tasks06();
        demo.RunMultipleAny().Wait();
        Console.WriteLine();
    }

    private async Task RunMultipleAny()
    {
        int maxPrime1 = 60_000;
        int maxPrime2 = 80_000;
        int num, avg;
        Stopwatch sw = new();

        sw.Start();
        var numTask = NumOfPrimesAsync(maxPrime1);
        var avgTask = AverageOfPrimesAsync(maxPrime2);
        var delayTask = Task.Delay(2000);
        var firstTask = await Task.WhenAny(numTask, avgTask, delayTask);
        var elapsed = sw.Elapsed;

        if (firstTask == numTask)
            Console.WriteLine($"First Num:{numTask.Result} TID:{Tid()} Elapsed:{elapsed}");
        else if(firstTask == avgTask)
            Console.WriteLine($"First Avg:{avgTask.Result} TID:{Tid()} Elapsed:{elapsed}");
        else
            Console.WriteLine($"First Timeout TID:{Tid()} Elapsed:{elapsed}");

        // Be careful: Result is a blocking property. The thread will block until Result is available
        num = numTask.Result;
        avg = avgTask.Result;

        Console.WriteLine($"All Num:{num} Avg:{avg} TID:{Tid()} Elapsed:{sw.Elapsed}");
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
