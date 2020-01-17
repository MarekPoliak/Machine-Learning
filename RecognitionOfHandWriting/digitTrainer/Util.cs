using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace digitTrainer
{
    public static class Util
    {
        public static Random Ran { get; set; } = new Random();

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

        public static void SaveData(string path, List<DigitData> digitData)
        {
            byte[] rawData = new byte[785*digitData.Count];
            int counter = 0;
            for (int imageIndex = 0; imageIndex < digitData.Count; imageIndex++)
            {
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        rawData[counter] = digitData[imageIndex].Pixels[i, j];
                        counter++;
                    }
                }
                rawData[counter] = digitData[imageIndex].ActualDigit;
                counter++;
            }
            File.WriteAllBytes(path, rawData);
        }

        public static double Sigmoid(double value)
        {
            return 1 / (1 + Math.Pow(Math.E, -value));
        }

        public static double RELu(double value)
        {
            return value < 0 ? 0 : value;
        }

        public static double GetRan()
        {
            return Util.Ran.Next(-10, 11) / 10.0;
        }

        public static double[] MatToArr(byte[,] matrix)
        {
            double[] result = new double[matrix.Length];
            int counter = 0;
            for (int i = 0; i < Math.Sqrt(matrix.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(matrix.Length); j++)
                {
                    result[counter] = matrix[i, j]>0?1:0;
                    counter++;
                }
            }
            return result;
        }

        public static void SaveNetworkData(string path, NeuralNetwork network)
        {
            var serializer = new XmlSerializer(typeof(NeuralNetwork));
            StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, network);
            writer.Close();
        }

        public static NeuralNetwork LoadNetworkData(string path)
        {
            var serializer = new XmlSerializer(typeof(NeuralNetwork));
            StreamReader reader = new StreamReader(path);
            var network=(NeuralNetwork)serializer.Deserialize(reader);
            reader.Close();
            network.SetNextLayers();
            return network;
        }

        public static TrainingData[] LoadTrainingData(List<DigitData> digitData, int count)
        {
            var trainData = new TrainingData[count];
            for (int i = 0; i < count; i++)
            {
                double[] output = new double[10];
                for (int j = 0; j < 10; j++)
                {
                    output[j] = 0.1;
                }
                output[digitData[i].ActualDigit] = 0.9;
                trainData[i] = new TrainingData(Util.MatToArr(digitData[i].Pixels), output);
            }
            return trainData;
        }
    }
}
