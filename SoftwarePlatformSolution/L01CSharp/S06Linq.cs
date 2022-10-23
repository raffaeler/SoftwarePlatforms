using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01CSharp;

internal class S06Linq
{
    public static void Run()
    {
        Filter();
        MapToDictionary();
        PrintAlphabet();
        MinMaxAnyAll();
        FilterByType();
    }

    private static void Filter()
    {
        List<int> sequence1 = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        var evenNumbers = sequence1
            .Where(n => n % 2 == 0);            // filter

        Console.WriteLine(string.Join(", ", evenNumbers));
        Console.WriteLine();
    }

    private static void MapToDictionary()
    {
        List<int> sequence1 = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        var evenNumbers = sequence1
            .Where(n => n % 2 == 0);            // filter

        var map = evenNumbers.ToDictionary(n => n, n => $"'{n}'");
        var mapStrings = map.Select(kvp => $"{kvp.Key}=>{kvp.Value}");
        Console.WriteLine(string.Join(", ", mapStrings));
        Console.WriteLine();
    }

    private static void PrintAlphabet()
    {
        // the ASCII character of 'A' is 65
        var chars = Enumerable.Range(65, 26)
            .Select(i => (char)i)               // projection
            .ToList();
        Console.WriteLine(string.Join(" ", chars));
        Console.WriteLine();
    }

    private static void MinMaxAnyAll()
    {
        // the _ character is used to "discard" the value
        var rnd = new Random();
        var randomSequence = Enumerable.Range(0, 10)
            .Select(_ => rnd.Next(50, 90))
            .ToArray();

        Console.WriteLine($"Random sequence:{string.Join(", ", randomSequence)}");
        Console.WriteLine($"Min:{randomSequence.Min()} Max:{randomSequence.Max()} Avg:{randomSequence.Average()}");
        Console.WriteLine($"Any above 85?: {randomSequence.Any(n => n > 85)}");
        Console.WriteLine($"All above 55?: {randomSequence.All(n => n > 55)}");
        Console.WriteLine();
    }

    private static void FilterByType()
    {
        List<object> mix = new() { "zero", 1, "two", 3, "four", 5, "six", 7, "eight", 9 };
        var justNumbers = mix
            .OfType<int>()
            .ToList();
        Console.WriteLine(string.Join(", ", justNumbers));
        Console.WriteLine();

        var justStrings = mix
            .OfType<string>()
            .ToList();
        Console.WriteLine(string.Join(", ", justStrings));
        Console.WriteLine();
    }
}
