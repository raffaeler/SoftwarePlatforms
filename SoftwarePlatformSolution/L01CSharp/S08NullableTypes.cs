using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp.S08;

internal class S08NullableTypes
{
    public static void Run()
    {
        Console.WriteLine($"{Add(null, null).PrintEx()}");
        Console.WriteLine($"{Add(1, null).PrintEx()}");
        Console.WriteLine($"{Add(null, 1).PrintEx()}");
        Console.WriteLine($"{Add(1, 1).PrintEx()}");

        Console.WriteLine();
    }

    private static double? Add(Nullable<double> x, double? y)
    {
        Console.Write($"{x.PrintEx()} + {y.PrintEx()} = ");

        if (x.HasValue && y.HasValue) return x.Value + y.Value;
        if (x.HasValue) return x.Value;
        if (!y.HasValue) return null;
        return y.Value;
    }

}

public static class Extensions
{
    public static string PrintEx(this double? value) => $"{(value.HasValue ? value : "null")}";
}
