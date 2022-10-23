using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp.S02;

internal class S02Classes
{
    public static void Run()
    {
        Console.WriteLine("C1: new and print");
        ClassOne c1 = new(1, 2);
        Console.WriteLine(c1);
        Console.WriteLine();

        Console.WriteLine("C2: new and print");
        ClassTwo c2 = new(3, 4);
        Console.WriteLine(c2);

        Console.WriteLine();
        Console.WriteLine();
    }


}


/// <summary>
/// Lives on the heap
/// </summary>
public class ClassOne
{
    public ClassOne(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"ClassOne => X:{X}, Y:{Y}";
    }
}


/// <summary>
/// Lives on the heap
/// The compiler automatically generates many convenient members
/// </summary>
public record class ClassTwo(int X, int Y);
