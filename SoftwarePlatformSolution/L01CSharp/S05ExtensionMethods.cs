using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp.S05;

internal class S05ExtensionMethods
{
    public static void Run()
    {
        int x = 10;
        var t1 = Extensions.Twice(x);
        var t2 = x.Twice();
        Console.WriteLine($"You get twice of {x} as {t1} or {t2}");
    }

}

public static class Extensions
{
    public static int Twice(this int number) => number * 2;
}
