using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp;

internal class S01Static
{
    public static void Run()
    {
        Console.WriteLine(_title);

        S01Static demo;
        demo = new();  // var demo = new S01Static(); // equivalent
        demo.Print();

        demo = new();
        demo.Print();

        demo = new();
        demo.Print();

        Console.WriteLine();
        Console.WriteLine();
    }

    private static string _title = "This is a demo";
    private DateTime _now = DateTime.Now;

    private static int _globalCount = 0;
    private static int _count = 0;

    public S01Static()
    {
        _count = ++_globalCount;
    }

    public void Print()
    {
        Console.WriteLine($"Instance {_count}. The current Date is: {_now.ToString()}");
    }

}
