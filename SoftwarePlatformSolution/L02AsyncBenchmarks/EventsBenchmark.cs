using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace L02AsyncBenchmarks;

public class EventsBenchmark
{
    private ManualResetEvent kernelMode = new ManualResetEvent(false);
    private ManualResetEventSlim userMode = new ManualResetEventSlim(false);

    [Benchmark]
    public void KernelModeEvent()
    {
        kernelMode.Set();
        kernelMode.Reset();
    }

    [Benchmark]
    public void UserModeEvent()
    {
        userMode.Set();
        userMode.Reset();
    }
}

/*

|          Method |        Mean |     Error |    StdDev |
|---------------- |------------:|----------:|----------:|
| KernelModeEvent | 2,131.34 ns | 37.847 ns | 45.054 ns |
|   UserModeEvent |    19.92 ns |  0.261 ns |  0.244 ns |





*/