using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02Async;

internal record ColorHelper : IDisposable
{
    private readonly ConsoleColor _foreground;

    public ColorHelper(ConsoleColor foreground)
    {
        _foreground = Console.ForegroundColor;
        Console.ForegroundColor = foreground;
    }

    public void Dispose()
    {
        Console.ForegroundColor = _foreground;
    }
}
