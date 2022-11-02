using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
namespace L02AsyncBenchmarks
{
    public class ThreadsBenchmark
    {
        public volatile int X = 1;
        public volatile int Y = 1;
        public volatile int Z = 1;
        public volatile int T = 1;

        [Benchmark]
        public void RunThread()
        {
            ManualResetEventSlim evt = new();
            Thread t = new Thread(() =>
            {
                X++;
                evt.Set();
            });
            t.Start();

            //t.Join();
            evt.Wait();
        }

        [Benchmark]
        public void RunPool()
        {
            ManualResetEventSlim evt = new();
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Y++;
                evt.Set();
            });

            evt.Wait();
        }

        [Benchmark]
        public void RunTaskEvt()
        {
            ManualResetEventSlim evt = new();
            Task.Run(() =>
            {
                Z++;
                evt.Set();
            });

            evt.Wait();
        }

        [Benchmark]
        public void RunTask()
        {
            Task.Run(() =>
            {
                T++;
            }).Wait();
        }

    }
}

/*

// Without runnning the threads/tasks

|    Method |       Mean |    Error |    StdDev |     Median |
|---------- |-----------:|---------:|----------:|-----------:|
| RunThread | 2,299.3 ns | 45.19 ns |  89.20 ns | 2,300.4 ns |
|   RunTask |   133.4 ns |  5.27 ns |  15.53 ns |   130.4 ns |


// Running the threads/tasks

|     Method |         Mean |       Error |      StdDev |
|----------- |-------------:|------------:|------------:|
|  RunThread | 128,952.3 ns | 2,571.97 ns | 2,526.02 ns |
|    RunPool |     942.2 ns |    18.51 ns |    20.58 ns |
| RunTaskEvt |     997.1 ns |     7.58 ns |     7.09 ns |
|    RunTask |     953.3 ns |    14.26 ns |    11.91 ns |


*/