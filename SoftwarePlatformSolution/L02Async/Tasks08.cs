using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal class Tasks08
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
            Console.WriteLine($"{nameof(RunTaskFault)} sample TID:{Tid()}");

        var demo = new Tasks08();
        demo.RunTaskFault().Wait();
        Console.WriteLine();
        Console.WriteLine();
        demo.RunAggregate();
    }

    private async Task RunTaskFault()
    {
        int maxPrime = 60_000;
        int explodesOn1 = 50;
        int explodesOn2 = 500;
        int num, avg;

        Task<int> numTask = NumOfPrimesAsync(maxPrime, explodesOn1);
        Task<int> avgTask = AverageOfPrimesAsync(maxPrime, explodesOn2);

        while (numTask.Status != TaskStatus.Faulted && numTask.Status != TaskStatus.RanToCompletion &&
            avgTask.Status != TaskStatus.Faulted && avgTask.Status != TaskStatus.RanToCompletion)
        {
            await Task.Delay(200);
        }

        // at this point the two tasks are already either completed or faulted
        // the exception happened but it was trapped by the state machine
        // If you were running the app under the debugger, it already paused on the
        // the execution to help you diagnosing the problem


        // Instead, without the debugger the execution continues as if there was no exception
        // until you either "await" the task or access the "Result" property
        Console.WriteLine($"{nameof(numTask)} is {numTask.Status}");
        Console.WriteLine($"{nameof(avgTask)} is {avgTask.Status}");

        try
        {
            num = await numTask;
            Console.WriteLine($"Num: {num}");
        }
        catch (Exception err)
        {
            using (_ = new ColorHelper(ConsoleColor.Red))
                Console.WriteLine($"{nameof(numTask)} fault is:");
            Console.WriteLine(err);
            Console.WriteLine();
        }

        try
        {
            avg = avgTask.Result;
            Console.WriteLine($"Avg: {avg}");
        }
        catch (Exception err)
        {
            using (_ = new ColorHelper(ConsoleColor.Red))
                Console.WriteLine($"{nameof(avgTask)} fault is:");
            Console.WriteLine(err);
            Console.WriteLine();
        }

    }

    private void RunAggregate()
    {
        int maxPrime = 60_000;
        int explodesOn1 = 50;
        int explodesOn2 = 500;
        int num, avg;

        Console.WriteLine();
        Console.WriteLine("RunAggregate() shows the AggregateException when using WaitAll()");
        Task<int> numTask = NumOfPrimesAsync(maxPrime, explodesOn1);
        Task<int> avgTask = AverageOfPrimesAsync(maxPrime, explodesOn2);

        try
        {
            Task.WaitAll(numTask, avgTask);
            num = numTask.Result;
            avg = avgTask.Result;
            Console.WriteLine($"Num: {num}");
            Console.WriteLine($"Avg: {avg}");
        }
        catch (AggregateException err)
        {
            using (_ = new ColorHelper(ConsoleColor.Red))
                Console.WriteLine("Multiple Exceptions occurred");
            foreach (var exception in err.InnerExceptions)
            {
                using (_ = new ColorHelper(ConsoleColor.DarkMagenta))
                    Console.WriteLine($"Fault:");
                Console.WriteLine($"{exception}");
            }
        }

    }


    private Task<int> NumOfPrimesAsync(int maxPrime, int explodesOn)
    {
        Task<int> computation = Task.Run(() =>
        {
            Console.WriteLine($"{nameof(NumOfPrimesAsync)} TID:{Tid()}");
            var primes = new Primes(maxPrime);
            var count = primes.Count();
            if (count > explodesOn)
            {
                throw new Exception($"Count exceeded {explodesOn}");
            }

            return count;
        });

        return computation;
    }

    private Task<int> AverageOfPrimesAsync(int maxPrime, int explodesOn)
    {
        Task<int> computation = Task.Run(() =>
        {
            Console.WriteLine($"{nameof(AverageOfPrimesAsync)} TID:{Tid()}");
            var primes = new Primes(maxPrime);
            var avg = (int)primes.Average();
            if (avg > explodesOn)
            {
                throw new Exception($"Average exceeded {explodesOn}");
            }

            return avg;
        });

        return computation;
    }

}
