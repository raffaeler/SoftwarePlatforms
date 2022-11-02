using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal class Tasks10
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
            Console.WriteLine($"{nameof(RunCompletionSource)} sample TID:{Tid()}");

        var demo = new Tasks10();
        demo.RunCompletionSource().Wait();
        Console.WriteLine();

        using (var _ = new ColorHelper(ConsoleColor.Yellow))
            Console.WriteLine($"{nameof(PrintTemperatures)} sample TID:{Tid()}");

        demo.PrintTemperatures().Wait();
        Console.WriteLine();
    }

    private async Task RunCompletionSource()
    {
        //_completionSource = new();

        Console.WriteLine($"Now: {DateTime.Now}");
        var delayed = await OneSecondDelay();
        Console.WriteLine($"Now: {delayed}");

    }

    private Task<DateTime> OneSecondDelay()
    {
        TaskCompletionSource<DateTime> completionSource = new();
        Timer timer = new(_ =>
        {
            completionSource.SetResult(DateTime.Now);
        });

        // timer will expire after 1s and not periodic
        timer.Change(1000, Timeout.Infinite);
        return completionSource.Task;
    }

    private async Task PrintTemperatures()
    {
        TemperatureSensor sensor = new();
        foreach(var i in Enumerable.Range(0, 10))
        {
            var value = await sensor.OnNextTemperature();
            Console.WriteLine($"T{i}: {value}");
        }
    }

}

public class TemperatureSensor
{
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;
    private Random _rnd = new();
    //public event EventHandler<double>? CurrentTemperature;
    private Queue<TaskCompletionSource<double>> _queue = new();
    private TaskCompletionSource<double> _next;
    private static object _sync = new object();
    private int _temp = 20;

    public TemperatureSensor()
    {
        _next = new();
        _queue.Enqueue(_next);

        Timer timer = new(OnTimer);

        // timer:
        // parameter 1 is the first occurrence in ms
        // parameter 2 period (1s)

        //timer.Change(0, 1000);     // produce first, consume then
        timer.Change(1000, 1000);    // consume first, produce then
    }

    private void OnTimer(object? _)
    {
        lock (_sync)
        {
            //Console.WriteLine($"OnTimer {Tid()}");
            var value = _rnd.Next(18, 24);
            if (!_queue.TryPeek(out TaskCompletionSource<double>? available))
            {
                throw new Exception($"{nameof(OnTimer)}: Unexpected empty queue");
            }

            if (available.Task.IsCompleted)
            {
                available = new();
                _queue.Enqueue(available);
                available.SetResult(value);
            }

            // The call to SetResult is a direct call that unblocks the result
            available.SetResult(value);

            // If you don't want the timer thread to be "busy" in calling the consumer thread,
            // you can offload the call using Task.Run
            //
            //Task.Run(() => available.SetResult(value));
        }
    }

    public Task<double> OnNextTemperature()
    {
        lock (_sync)
        {
            //Console.WriteLine($"OnNextTemperature {Tid()}");
            if (!_queue.TryPeek(out TaskCompletionSource<double>? available))
            {
                throw new Exception($"{nameof(OnNextTemperature)}: Unexpected empty queue");
            }

            if (available.Task.IsCompleted)
            {
                _queue.Dequeue();
                if (_queue.Count == 0)
                {
                    available = new();
                    _queue.Enqueue(available);
                }
            }
            else
            {
                Console.WriteLine("Reading NotCompleted");
            }

            return available.Task;
        }
    }

}
