using System;
using System.Diagnostics;

namespace ScrapporNet.Extensions
{
    public static class StopWatchExtensions
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

        public static int RoundOff(this int i)
        {
            return ((int)Math.Round(i / 1024.00)) * 1024;
        }
    }
}
