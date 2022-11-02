using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02AsyncBenchmarks
{
    internal class ThreadCreation
    {
        public static void Run()
        {
            int count = 100_000;
            Stopwatch sw = new();
            sw.Start();

            for(int i=0; i<count; i++)
            {
                Thread thread = new(() =>
                {
                });
            }

            sw.Stop();
            var threadElapsed = sw.Elapsed;
            Console.WriteLine($"Thread count:{count} => {threadElapsed}");

            sw.Restart();

            for (int i = 0; i < count; i++)
            {
                Task t = new Task(() => { });
            }

            sw.Stop();
            var taskElapsed = sw.Elapsed;
            Console.WriteLine($"Task count:  {count} => {taskElapsed}");

            Console.WriteLine($"Difference = {threadElapsed - taskElapsed}");
        }
    }
}
