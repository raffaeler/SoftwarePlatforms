using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Offloading a CPU-intensive operation on a different thread
// Using async/await to return the result as soon as it is available

namespace L02Async;

internal class Tasks04
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"{nameof(RunCPUBound)} sample TID:{Tid()}");
        }

        var demo = new Tasks04();
        demo.RunCPUBound().Wait();
        Console.WriteLine();
    }

    private async Task RunCPUBound()
    {
        int maxPrime = 60_000;
        int result;
        Stopwatch sw = new();

        sw.Start();
        result = NumOfPrimes(maxPrime);
        sw.Stop();

        Console.WriteLine($"Sync  - Result is {result} TID:{Tid()} Elapsed:{sw.Elapsed}");

        sw.Restart();
        result = await NumOfPrimesAsync(maxPrime);
        sw.Stop();

        Console.WriteLine($"Async - Result is {result} TID:{Tid()} Elapsed:{sw.Elapsed}");
    }


    private int NumOfPrimes(int maxPrime)
    {
        var primes = new Primes(maxPrime);
        return primes.Count();
    }

    private Task<int> NumOfPrimesAsync(int maxPrime)
    {
        Task<int> computation = Task.Run(() =>
        {
            var primes = new Primes(maxPrime);
            return primes.Count();
        });

        return computation;
    }


}
