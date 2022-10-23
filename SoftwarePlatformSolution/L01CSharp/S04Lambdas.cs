using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp;

internal class S04Lambdas
{
    public static void Run()
    {
        var demo = new S04Lambdas();

        demo.DoOperation1(5, i => $"Number {i}");

        demo.DoOperation1(3, i =>
        {
            return $"N: {i}";
        });

        // "fffffff" prints the ten-millionths of a second
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings#f7Specifier
        demo.DoOperation2(3, (i, time) => $"Num:{i}, Time: {time:fffffff}");
    }


    private int Add1(int a, int b)
    {
        return a + b;
    }

    private int Add2(int a, int b) => a + b;

    private Func<int, int, int> Add3 = (a, b) => a + b;

    private Func<int, int, int> Add4 = (int a, int b) => a + b;


    private void DoOperation1(int max, Func<int, string> callback)
    {
        for (int i = 0; i < max; i++)
        {
            var result = callback(i);
            Console.WriteLine(result);
        }
    }

    private void DoOperation2(int max, Func<int, TimeSpan, string> callback)
    {
        var sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < max; i++)
        {
            var result = callback(i, sw.Elapsed);
            Console.WriteLine(result);
        }
    }

}
