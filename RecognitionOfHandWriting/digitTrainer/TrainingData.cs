using System;
using System.Collections.Generic;
using System.Text;

namespace digitTrainer
{
    public class TrainingData
    {
        public double[] Input { get; set; }
        public double[] Output { get; set; }

        public TrainingData(double[] input, double[] output)
        {
            Input = input;
            Output = output;
        }
    }
}
