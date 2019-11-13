using System;
using System.Collections.Generic;
using System.Text;

namespace FirstML
{
    public static class ML
    {
        public static TrainingData[] Inputs { get; set; }
        public static Layer InputLayer { get; set; }
        public static Layer HiddenLayer { get; set; }
        public static Layer OutputLayer { get; set; }
        public static float LearningRate { get; set; } = 0.5f;

        public static void Init()
        {
            Inputs = new TrainingData[]
            {
                new TrainingData(0,0,0),
                new TrainingData(0,1,1),
                new TrainingData(1,0,1),
                new TrainingData(1,1,0)
            };
            InputLayer = new Layer(new Neuron[2]
            {
                new Neuron(0,null),
                new Neuron(0,null)
            });
            HiddenLayer = new Layer(new Neuron[3]
            {
                new Neuron(2,null),
                new Neuron(2,null),
                new Neuron(2,null)
            });
            OutputLayer = new Layer(new Neuron[1]
            {
                new Neuron(3,null)
            });
            OutputLayer.Neurons[0].Bias = 1;
        }

        public static void Show()
        {
            Console.WriteLine("----INPUT LAYER----");
            foreach (var item in InputLayer.Neurons)
            {
                Console.WriteLine("Value: " + item.Value);
            }
            Console.WriteLine("----HIDDEN LAYER----");
            foreach (var item in HiddenLayer.Neurons)
            {
                Console.WriteLine("Value: " + item.Value);
                Console.WriteLine("Bias: " + item.Bias);
                Console.WriteLine("Num of Weights: " + item.Weights.Length);
                foreach (var weight in item.Weights)
                {
                    Console.WriteLine("weight: " + weight);
                }
            }
            Console.WriteLine("----OUTPUT LAYER----");
            foreach (var item in OutputLayer.Neurons)
            {
                Console.WriteLine("Value: " + item.Value);
                Console.WriteLine("Num of Weights: " + item.Weights.Length);
                foreach (var weight in item.Weights)
                {
                    Console.WriteLine("weight: " + weight);
                }
            }
        }

        public static void Proccess()
        {
            if (InputLayer.Neurons[0].Value.HasValue && InputLayer.Neurons[1].Value.HasValue)
            {
                foreach (var neuron in HiddenLayer.Neurons)
                {
                    neuron.Net = (neuron.Weights[0] * InputLayer.Neurons[0].Value + neuron.Weights[1] * InputLayer.Neurons[1].Value) + neuron.Bias;
                    neuron.Value = Util.Sigmoid(neuron.Net);
                }
                for (int i = 0; i < OutputLayer.Neurons.Length; i++)
                {
                    OutputLayer.Neurons[i].Net = (OutputLayer.Neurons[i].Weights[0] * HiddenLayer.Neurons[0].Value + OutputLayer.Neurons[i].Weights[1] * HiddenLayer.Neurons[1].Value + OutputLayer.Neurons[i].Weights[2] * HiddenLayer.Neurons[2].Value) + OutputLayer.Neurons[i].Bias;
                    OutputLayer.Neurons[i].Value = Util.Sigmoid(OutputLayer.Neurons[i].Net);
                }
            }
        }

        public static void InsertInput(TrainingData data)
        {
            InputLayer.Neurons[0].Value = data.Input[0];
            InputLayer.Neurons[1].Value = data.Input[1];
        }

        public static void BackPropagation(TrainingData data)
        {
            /*for (int i = 0; i < OutputLayer.Neurons[0].Weights.Length; i++)
            {
                float? ChanOutError = (float?)(0.5 * Math.Pow(((double)(data.Output - OutputLayer.Neurons[0].Value)), 2));
                float? ChanNetOut = OutputLayer.Neurons[0].Value * (1 - OutputLayer.Neurons[0].Value);
                float? ChanWeightNet = OutputLayer.Neurons[0].Weights[0] * HiddenLayer.Neurons[0].Value + OutputLayer.Neurons[0].Weights[1] * HiddenLayer.Neurons[1].Value + OutputLayer.Neurons[0].Weights[2] * HiddenLayer.Neurons[2].Value + OutputLayer.Neurons[0].Bias;
                float? weightGradient = ChanOutError * ChanNetOut * ChanWeightNet;
                OutputLayer.Neurons[0].Weights[i] -= LearningRate * weightGradient;
            }
            for (int i = 0; i < HiddenLayer.Neurons.Length; i++)
            {
                for (int j = 0; j < HiddenLayer.Neurons[i].Weights.Length; j++)
                {
                    float? ChanOutError = (float?)(0.5 * Math.Pow(((double)(data.Output - HiddenLayer.Neurons[i].Value)), 2));
                    float? ChanNetOut = HiddenLayer.Neurons[i].Value * (1 - HiddenLayer.Neurons[i].Value);
                    float? ChanWeightNet = HiddenLayer.Neurons[i].Weights[0] * InputLayer.Neurons[0].Value + HiddenLayer.Neurons[i].Weights[1] * InputLayer.Neurons[1].Value + +HiddenLayer.Neurons[i].Bias;
                    float? weightGradient = ChanOutError * ChanNetOut * ChanWeightNet;
                    HiddenLayer.Neurons[i].Weights[j] -= LearningRate * weightGradient;
                }
            }*/
        }

        public static void Mutate(int index)
        {
            ML.InsertInput(ML.Inputs[index]);
            ML.Proccess();
            float error = Math.Abs((float)(Inputs[index].Output - OutputLayer.Neurons[0].Value));
            if (error < 0.01)
            {
                return;
            }
            int HWI = Program.Ran.Next(0, HiddenLayer.Neurons[0].Weights.Length);
            int OWI = Program.Ran.Next(0, OutputLayer.Neurons[0].Weights.Length);
            int HNI = Program.Ran.Next(0, HiddenLayer.Neurons.Length);
            int ONI = Program.Ran.Next(0, OutputLayer.Neurons.Length);
            float HWV = Program.Ran.Next(-10, 11) / 100.0f;
            float OWV = Program.Ran.Next(-10, 11) / 100.0f;
            float HBV = Program.Ran.Next(-10, 11) / 100.0f;
            HiddenLayer.Neurons[HNI].Weights[HWI] += HWV;
            HiddenLayer.Neurons[HNI].Bias += HBV;
            OutputLayer.Neurons[ONI].Weights[OWI] += OWV;
            ML.InsertInput(ML.Inputs[index]);
            ML.Proccess();
            float newError = Math.Abs((float)(Inputs[index].Output - OutputLayer.Neurons[0].Value));
            if (newError > error)
            {
                HiddenLayer.Neurons[HNI].Weights[HWI] -= HWV;
                HiddenLayer.Neurons[HNI].Bias -= HBV;
                OutputLayer.Neurons[ONI].Weights[OWI] -= OWV;
            }
        }

        public static void ShowResult()
        {
            Console.WriteLine("Value: " + OutputLayer.Neurons[0].Value);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
