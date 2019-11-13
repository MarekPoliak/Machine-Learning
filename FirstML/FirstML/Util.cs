using System;
using System.Collections.Generic;
using System.Text;

namespace FirstML
{
    public static class Util
    {
        public static float? RanNum(int min, int max)
        {
            return Program.Ran.Next(min*100, max*100 + 1) / (float?)100.0;
        }

        public static float? Sigmoid(float? input)
        {
            return (float?)(1 / (1 + Math.Pow(Math.E, -input.Value)));
        }
    }
}
