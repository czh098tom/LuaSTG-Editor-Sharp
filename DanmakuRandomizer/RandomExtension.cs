using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmakuRandomizer
{
    internal static class RandomExtension
    {
        public static double NextDecayedDouble(this Random random, double lowerBound, double upperBound)
        {
            double dist = upperBound - lowerBound;
            return random.NextDouble() * random.NextDouble() * dist + lowerBound;
        }

        public static double NextReversedDecayedDouble(this Random random, double lowerBound, double upperBound)
        {
            double dist = upperBound - lowerBound;
            return (1 - random.NextDouble() * random.NextDouble()) * dist + lowerBound;
        }
    }
}
