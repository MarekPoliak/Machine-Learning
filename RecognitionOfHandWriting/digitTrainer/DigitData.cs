using System;
using System.Collections.Generic;
using System.Text;

namespace digitTrainer
{
    public class DigitData
    {
        public byte[,] Pixels { get; set; }
        public byte ActualDigit { get; set; }

        public DigitData()
        {
            Pixels = new byte[28,28];
        }

        public static void ShowDigit(double[] data)
        {
            int counter = 0;
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    Console.Write(data[counter]==1?"#":".");
                    counter++;
                }
                Console.WriteLine();
            }
        }
    }
}
