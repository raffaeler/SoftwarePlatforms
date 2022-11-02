using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
namespace L02AsyncBenchmarks
{
    public class ThreadsBenchmark2
    {
        private const int Repeat = 64;
        private const int Loop = 50000;

        [Params(1, 8, 32767)]
        public int N;

        [GlobalSetup]
        public void Init()
        {
            int wmin, cmin;
            int wmax, cmax;
            ThreadPool.GetMinThreads(out wmin, out cmin);
            ThreadPool.GetMaxThreads(out wmax, out cmax);
            Console.WriteLine($"Min: {wmin}, {cmin}");
            Console.WriteLine($"Max: {wmax}, {cmax}");
            Console.WriteLine();
            Console.WriteLine();
            var ismin = ThreadPool.SetMinThreads(N, cmin);
            var ismax = ThreadPool.SetMaxThreads(N, cmax);

            if (!ismin)
                throw new Exception($"Invalid Min thread value {N}");
            if (!ismax)
                throw new Exception($"Invalid Max thread value {N}");

            ThreadPool.GetMinThreads(out wmin, out cmin);
            ThreadPool.GetMaxThreads(out wmax, out cmax);

            Console.WriteLine($"Using Min: {wmin}, {cmin}");
            Console.WriteLine($"Using Max: {wmax}, {cmax}");

            Console.WriteLine();
            Console.WriteLine();
        }

        public volatile int X = 1;
        public volatile int Y = 1;
        public volatile int Z = 1;

        [Benchmark]
        public void RunThread()
        {
            ManualResetEventSlim evtStart = new();

            var handles = new WaitHandle[Repeat];
            for (int i = 0; i < Repeat; i++)
            {
                var evt = new ManualResetEvent(false);
                handles[i] = evt;
                Thread t = new Thread(() =>
                {
                    evtStart.Wait();
                    for(int j=0; j<Loop; j++)
                        X++;
                    evt.Set();
                });
                t.Start();
            }

            evtStart.Set();
            WaitHandle.WaitAll(handles);
        }

        [Benchmark]
        public void RunPool()
        {
            ManualResetEventSlim evtStart = new();

            var handles = new WaitHandle[Repeat];
            for (int i = 0; i < Repeat; i++)
            {
                var evt = new ManualResetEvent(false);
                handles[i] = evt;
                var queued = ThreadPool.QueueUserWorkItem(_ =>
                {
                    evtStart.Wait();
                    for (int j = 0; j < Loop; j++)
                        Y++;
                    evt.Set();
                });
                if (!queued) throw new Exception($"failed to queue the operation");
            }

            evtStart.Set();
            WaitHandle.WaitAll(handles);
        }

        [Benchmark]
        public void RunTaskEvt()
        {
            ManualResetEventSlim evtStart = new();

            var handles = new WaitHandle[Repeat];
            for (int i = 0; i < Repeat; i++)
            {
                var evt = new ManualResetEvent(false);
                handles[i] = evt;
                Task.Run(() =>
                {
                    evtStart.Wait();
                    for (int j = 0; j < Loop; j++)
                        Z++;
                    evt.Set();
                });
            }

            evtStart.Set();
            WaitHandle.WaitAll(handles);
        }


    }
}

/*

// Run on a 8 core CPU with Loop = 50000

|     Method |     N |      Mean |     Error |    StdDev |
|----------- |------ |----------:|----------:|----------:|
|  RunThread |     1 | 10.998 ms | 0.1152 ms | 0.0962 ms |
|    RunPool |     1 |  5.227 ms | 0.0426 ms | 0.0378 ms |
| RunTaskEvt |     1 |  5.216 ms | 0.0408 ms | 0.0340 ms |
|----------- |------ |----------:|----------:|----------:|
|  RunThread |     8 | 11.051 ms | 0.2078 ms | 0.2223 ms |
|    RunPool |     8 |  3.042 ms | 0.0069 ms | 0.0065 ms |
| RunTaskEvt |     8 |  3.035 ms | 0.0231 ms | 0.0216 ms |
|----------- |------ |----------:|----------:|----------:|
|  RunThread | 32767 | 10.846 ms | 0.1018 ms | 0.0902 ms |
|    RunPool | 32767 |  3.267 ms | 0.0409 ms | 0.0363 ms |
| RunTaskEvt | 32767 |  3.238 ms | 0.0434 ms | 0.0363 ms |



// Run on a 8 core CPU with Loop = 100000

|     Method |     N |      Mean |     Error |    StdDev |
|----------- |------ |----------:|----------:|----------:|
|  RunThread |     1 | 13.756 ms | 0.1366 ms | 0.1067 ms |
|    RunPool |     1 | 10.037 ms | 0.0521 ms | 0.0487 ms |
| RunTaskEvt |     1 | 10.039 ms | 0.0863 ms | 0.0765 ms |
|----------- |------ |----------:|----------:|----------:|
|  RunThread |     8 | 13.663 ms | 0.2323 ms | 0.1940 ms |
|    RunPool |     8 |  5.875 ms | 0.0786 ms | 0.0735 ms |
| RunTaskEvt |     8 |  5.941 ms | 0.0243 ms | 0.0228 ms |
|----------- |------ |----------:|----------:|----------:|
|  RunThread | 32767 | 13.448 ms | 0.2060 ms | 0.1927 ms |
|    RunPool | 32767 |  6.062 ms | 0.1035 ms | 0.0917 ms |
| RunTaskEvt | 32767 |  6.083 ms | 0.0582 ms | 0.0544 ms |


// Run on a 8 core CPU with Loop = 1000000

|     Method |     N |     Mean |    Error |   StdDev |
|----------- |------ |---------:|---------:|---------:|
|  RunThread |     1 | 64.29 ms | 1.220 ms | 1.498 ms |
|    RunPool |     1 | 96.10 ms | 0.450 ms | 0.421 ms |
| RunTaskEvt |     1 | 95.94 ms | 0.495 ms | 0.414 ms |
|----------- |------ |---------:|---------:|---------:|
|  RunThread |     8 | 63.96 ms | 1.026 ms | 0.910 ms |
|    RunPool |     8 | 59.06 ms | 0.388 ms | 0.344 ms |
| RunTaskEvt |     8 | 56.88 ms | 0.805 ms | 0.753 ms |
|----------- |------ |---------:|---------:|---------:|
|  RunThread | 32767 | 64.66 ms | 1.272 ms | 1.128 ms |
|    RunPool | 32767 | 56.79 ms | 1.120 ms | 1.047 ms |
| RunTaskEvt | 32767 | 56.38 ms | 0.657 ms | 0.614 ms |
*/