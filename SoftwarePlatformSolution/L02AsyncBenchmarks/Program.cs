using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace L02AsyncBenchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        Summary summary;

        //ThreadCreation.Run();

        //summary = BenchmarkRunner.Run<EventsBenchmark>();
        //summary = BenchmarkRunner.Run<ThreadsBenchmark>();
        summary = BenchmarkRunner.Run<ThreadsBenchmark2>();
        Console.ReadKey();
    }
}