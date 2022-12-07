using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

// The "fire and forget" pattern: "async void"

namespace L02Async;

internal class Tasks07
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"{nameof(RunFireAndForget)} sample TID:{Tid()}");
        }

        var demo = new Tasks07();
        demo.RunFireAndForget().Wait();
        Console.WriteLine();
    }

    private async Task RunFireAndForget()
    {
        int maxPrime1 = 60_000;
        int maxPrime2 = 40_000;
        Stopwatch sw = new();

        sw.Start();
        LastPrimeAndForgetAsync(maxPrime1);
        await LastPrimeAsync(maxPrime2);
        sw.Stop();

        Console.WriteLine($"FireAndForget TID:{Tid()} Elapsed:{sw.Elapsed}");

        // The LastPrimeAndForgetAsync is still executing.
        // We wait for the user to press a key to avoid the program terminating before
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }


    private Task LastPrimeAsync(int maxPrime)
    {
        Task computation = Task.Run(() =>
        {
            Console.WriteLine($"{nameof(LastPrimeAsync)} started. TID:{Tid()}");
            var primes = new Primes(maxPrime);
            var result = (int)primes.Last();
            Console.WriteLine($"{nameof(LastPrimeAsync)} completed. Result:{result} TID:{Tid()}");
        });

        return computation;
    }

    private void LastPrimeAndForgetAsync(int maxPrime)
    {
        Task.Run(() =>
        {
            Console.WriteLine($"{nameof(LastPrimeAndForgetAsync)} started. TID:{Tid()}");
            var primes = new Primes(maxPrime);
            var result = (int)primes.Last();
            Console.WriteLine($"{nameof(LastPrimeAndForgetAsync)} completed. Result:{result} TID:{Tid()}");
        });
    }


}
