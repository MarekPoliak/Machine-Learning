using digitTrainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {

            var croppedDigitData = Util.LoadData("../../../data/croppedTrainingData.txt");
            var digitData = Util.LoadData("../../../data/trainingData.txt");
            for (int i = 0; i < digitData.Count; i++)
            {

                DigitData.ShowDigit(Util.MatToArr(digitData[i].Pixels));
                Console.WriteLine();
                DigitData.ShowDigit(Util.MatToArr(croppedDigitData[i].Pixels));
                Console.ReadLine();

            }
        }
    }
}
