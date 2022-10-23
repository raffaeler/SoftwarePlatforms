using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp.S03;

internal class S03Structs
{
    public static void Run()
    {
        StructOne s1 = new(50, 60);
        Console.WriteLine(s1);
        Console.WriteLine();

        Console.WriteLine("S2: new and print");
        StructTwo s2 = new(70, 80);
        Console.WriteLine(s2);

        Console.WriteLine();
        Console.WriteLine();
    }
}

/// <summary>
/// Lives on the stack
/// </summary>
public struct StructOne
{
    public StructOne()
        : this(0, 0)
    {
    }

    public StructOne(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"StructOne => X:{X}, Y:{Y}";
    }
}

/// <summary>
/// Lives on the stack
/// The compiler automatically generates many convenient members
/// </summary>
public record struct StructTwo(int X, int Y);
