using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ScrapporNet
{
    public static class StopwatchExtensions
    {
        public static long Time(this Stopwatch sw, Action action, int iterations)
        {
            sw.Reset();
            sw.Start();
            for (var i = 0; i < iterations; i++)
            {
                action();
            }
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }
}
