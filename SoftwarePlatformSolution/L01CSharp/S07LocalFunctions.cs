using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp;

internal class S07LocalFunctions
{
    public static void Run()
    {
        double a = 2, b = -5, c = -1;
        var (s1, s2) = ComputeSecondOrderEquation(a, b, c);
        Console.WriteLine($"Equation: {a}x{b}y{c}=0");
        Console.WriteLine($"Solutions: ({s1}, {s2})");
    }

    public static (double r1, double? r2) ComputeSecondOrderEquation(double a, double b, double c)
    {
        if (a == 0 && b == 0) throw new InvalidOperationException($"Not a valid equation");
        if (a == 0) return (-c / b, null);

        var radicand = b * b - 4 * a * c;
        if (radicand < 0) throw new InvalidOperationException("No real solutions");

        double radix = Math.Sqrt(radicand);
        double aa = 2 * a;
        double sol1 = calc(aa, b, radix);
        double sol2 = calc(aa, b, -radix);
        return (sol1, sol2);

        static double calc(double aa, double b, double radix) => (-b + radix) / aa;
    }

}
