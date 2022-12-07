using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Continuations using C# language support: async/await keywords

namespace L02Async;

internal class Tasks03
{
    private readonly string _filename = "DeBelloGallico_LiberI.txt";
    private static Func<int> Tid = () => Thread.CurrentThread.ManagedThreadId;

    public static void Run()
    {
        using (var _ = new ColorHelper(ConsoleColor.Yellow))
        {
            Console.WriteLine($"{nameof(LanguageContinuation)} sample TID:{Tid()}");
        }

        var demo = new Tasks03();
        demo.LanguageContinuation().Wait();
        Console.WriteLine();
    }

    private async Task LanguageContinuation()
    {
        int length = await ReadLengthAsync(_filename);

        Console.WriteLine($"Result is {length} TID:{Tid()}");
    }

    private async Task<int> ReadLengthAsync(string filename)
    {
        string content = await File.ReadAllTextAsync(filename);
        return content.Length;
    }
}
