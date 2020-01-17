using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace digitTrainer
{
    public class Program
    {
        public static string TrainDataPath { get; set; } = "../../../data/croppedTrainingData.txt";
        public static string NetworkDataPath { get; set; } = "../../../data/networkData2.xml";


        public static void Main(string[] args)
        {
            var digitData = Util.LoadData(TrainDataPath);
            var trainData = Util.LoadTrainingData(digitData, 60000);
            var network = Util.LoadNetworkData(NetworkDataPath);
            network.LearningRate = 0.01;
            network.BackPropagation(trainData);
            Util.SaveNetworkData(NetworkDataPath, network);
            Console.ReadLine();
        }
    }
}