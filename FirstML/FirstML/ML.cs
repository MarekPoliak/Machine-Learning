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
            InputLayer = new Layer(new Neuron[]
            {
                new Neuron(0,null),
                new Neuron(0,null)
            });
            HiddenLayer = new Layer(new Neuron[]
            {
                new Neuron(2,null),
                new Neuron(2,null),
                new Neuron(2,null)
            });
            OutputLayer = new Layer(new Neuron[]
            {
                new Neuron(3,null)
            });
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
                Console.WriteLine("Bias: " + item.Bias);
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
                    OutputLayer.Neurons[i].Net = OutputLayer.Neurons[i].Weights[0] * HiddenLayer.Neurons[0].Value + OutputLayer.Neurons[i].Weights[1] * HiddenLayer.Neurons[1].Value + OutputLayer.Neurons[i].Weights[2] * HiddenLayer.Neurons[2].Value + OutputLayer.Neurons[i].Bias;
                    OutputLayer.Neurons[i].Value = Util.Sigmoid(OutputLayer.Neurons[i].Net);
                }
            }
        }

        public static void InsertInput(TrainingData data)
        {
            InputLayer.Neurons[0].Value = data.Input[0];
            InputLayer.Neurons[1].Value = data.Input[1];
        }

        public static void InsertInput(int i1, int i2)
        {
            InputLayer.Neurons[0].Value = i1;
            InputLayer.Neurons[1].Value = i2;
        }

        public static void Mutate(int index)
        {
            TrainingData input = ML.Inputs[index];
            ML.InsertInput(input);
            ML.Proccess();
            float error = Math.Abs((float)(input.Output - OutputLayer.Neurons[0].Value));
            if (error < 0.01)
            {
                return;
            }
            int HWI = Program.Ran.Next(0, HiddenLayer.Neurons[0].Weights.Length);
            int OWI = Program.Ran.Next(0, OutputLayer.Neurons[0].Weights.Length);
            int HNI = Program.Ran.Next(0, HiddenLayer.Neurons.Length);
            int ONI = Program.Ran.Next(0, OutputLayer.Neurons.Length);
            float HWV = Program.Ran.Next(-100, 101) / 100.0f;
            float OWV = Program.Ran.Next(-100, 101) / 100.0f;
            float HBV = Program.Ran.Next(-100, 101) / 100.0f;
            HiddenLayer.Neurons[HNI].Weights[HWI] += HWV;
            HiddenLayer.Neurons[HNI].Bias += HBV;
            OutputLayer.Neurons[ONI].Weights[OWI] += OWV;
            ML.InsertInput(input);
            ML.Proccess();
            float newError = Math.Abs((float)(input.Output - OutputLayer.Neurons[0].Value));
            if (newError > error)
            {
                HiddenLayer.Neurons[HNI].Weights[HWI] -= HWV;
                HiddenLayer.Neurons[HNI].Bias -= HBV;
                OutputLayer.Neurons[ONI].Weights[OWI] -= OWV;
            }
        }

        public static bool IsSmart()
        {
            for (int i = 0; i < 4; i++)
            {
                ML.InsertInput(ML.Inputs[i]);
                ML.Proccess();
                if(!(OutputLayer.Neurons[0].Value + 0.01>ML.Inputs[i].Output) || !(OutputLayer.Neurons[0].Value - 0.01 < ML.Inputs[i].Output))
                {
                    return false;
                }
            }
            return true;
        }

        public static void ShowResults(int i)
        {
            Console.WriteLine("input: " + ML.Inputs[i].Input[0] + " " + ML.Inputs[i].Input[1]);
            Console.WriteLine("Expected: " + ML.Inputs[i].Output + "  Guess: " + ML.OutputLayer.Neurons[0].Value);
            Console.WriteLine();
        }
    }
}
