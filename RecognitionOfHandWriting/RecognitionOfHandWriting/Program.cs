using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecognitionOfHandWriting
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainData = new TrainingData[]
            {
                //new TrainingData(new double[]{1,1,1 },new double[]{1,1}),
                new TrainingData(new double[]{0,0,0 },new double[]{0,0}),
                new TrainingData(new double[]{1,0,1 },new double[]{1,1}),
                //new TrainingData(new double[]{0,0,1 },new double[]{0,1}),
                //new TrainingData(new double[]{1,0,0 },new double[]{1,0})
            };
            NeuralNetwork network = new NeuralNetwork(3, 2, 2, 4);
            network.SlowLearn(trainData);
            network.TestIt(trainData);
        }
    }
}