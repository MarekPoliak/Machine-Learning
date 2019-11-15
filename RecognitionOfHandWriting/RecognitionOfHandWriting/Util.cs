using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RecognitionOfHandWriting
{
    static class Util
    {
        public static List<DigitData> LoadData(string path)
        {
            var digitData = new List<DigitData>();
            byte[] rawData = File.ReadAllBytes(path);
            int counter = 0;
            for (int imageIndex = 0; imageIndex < rawData.Length / 785; imageIndex++) // 785 = imageWidth*imageHeight + 1 --- 1 is for the actual digit
            {
                digitData.Add(new DigitData());
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        digitData[imageIndex].Pixels[i, j] = rawData[counter];
                        counter++;
                    }
                }
                digitData[imageIndex].ActualDigit = rawData[counter];
                counter++;
            }
            return digitData;
        }
    }
}
