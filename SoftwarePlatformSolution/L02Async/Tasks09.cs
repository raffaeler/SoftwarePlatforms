using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal class Tasks09
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
            Console.WriteLine($"{nameof(RunCancellation1)} sample TID:{Tid()}");

        var demo = new Tasks09();
        demo.RunCancellation1().Wait();
        Console.WriteLine();
    }

    private async Task RunCancellation1()
    {
        int maxPrime = 200_000;
        CancellationTokenSource cts = new();

        try
        {
            Task printTask = PrintPrimes(maxPrime, cts.Token);
            await Task.Delay(100);

            if (!printTask.IsCompleted)
                cts.Cancel();   // causes a OperationCanceledException

            await printTask;    // this throws the OperationCanceledException if the token was canceled
            Console.WriteLine("no cancellation");
        }
        catch (OperationCanceledException err)
        {
            Console.WriteLine();
            using (_ = new ColorHelper(ConsoleColor.Red))
                Console.WriteLine($"Task was canceled");
            Console.WriteLine(err);
            Console.WriteLine();
        }
    }

    private Task PrintPrimes(int maxPrime, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            foreach (var prime in new Primes(maxPrime, cancellationToken))
            {
                if (prime == 1)
                    Console.Write($"{prime}");
                else
                    Console.Write($",{prime}");
            }
        });
    }

    private Task<int> NumOfPrimesAsync(int maxPrime, CancellationToken cancellationToken = default)
    {
        Task<int> computation = Task.Run(() =>
        {
            Console.WriteLine($"{nameof(NumOfPrimesAsync)} TID:{Tid()}");
            var primes = new Primes(maxPrime, cancellationToken);
            return primes.Count();
        });

        return computation;
    }

    //private Task<int> AverageOfPrimesAsync(int maxPrime)
    //{
    //    Task<int> computation = Task.Run(() =>
    //    {
    //        Console.WriteLine($"{nameof(AverageOfPrimesAsync)} TID:{Tid()}");
    //        var primes = new Primes(maxPrime);
    //        return (int)primes.Average();
    //    });

    //    return computation;
    //}

}
