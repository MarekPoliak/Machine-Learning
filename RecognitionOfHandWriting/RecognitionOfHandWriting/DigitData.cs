using System;
using System.Collections.Generic;
using System.Text;

namespace RecognitionOfHandWriting
{
    class DigitData
    {
        public byte[,] Pixels { get; set; }
        public byte ActualDigit { get; set; }

        public DigitData()
        {
            Pixels = new byte[28,28];
        }
    }
}
