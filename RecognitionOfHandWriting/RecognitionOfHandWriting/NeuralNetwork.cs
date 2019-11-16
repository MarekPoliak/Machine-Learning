using System;
using System.Collections.Generic;
using System.Text;

namespace RecognitionOfHandWriting
{
    class NeuralNetwork
    {
        public Layer InputLayer { get; set; }
        public Layer OutputLayer { get; set; }
        public Layer[] HiddenLayers { get; set; }
        public List<DataWrapper> NetworkData { get; set; } // weights and biases
        public Mutation LastMutation { get; set; }

        public NeuralNetwork(int numOfInputNeurons, int numOfOutputNeurons, int numOfHiddenLayers, int numOfHidNeurPerLayer)
        {
            LastMutation = new Mutation();
            NetworkData = new List<DataWrapper>();
            InputLayer = new Layer(numOfInputNeurons);
            OutputLayer = new Layer(numOfOutputNeurons, numOfHidNeurPerLayer, NetworkData);
            HiddenLayers = new Layer[numOfHiddenLayers];
            HiddenLayers[0] = new Layer(numOfHidNeurPerLayer, numOfInputNeurons, NetworkData);
            for (int i = 1; i < numOfHiddenLayers; i++)
            {
                HiddenLayers[i] = new Layer(numOfHidNeurPerLayer, numOfHidNeurPerLayer, NetworkData);
            }
        }

        public void InsertInput(double[] inputValues)
        {
            if (inputValues.Length != InputLayer.NumOfNeurons)
            {
                Console.WriteLine("bad num of inputs");
                Console.ReadLine();
                return;
            }
            for (int i = 0; i < inputValues.Length; i++)
            {
                InputLayer.Neurons[i].Value = inputValues[i];
            }
        }

        public void FeedForward()
        {
            if (!InputLayer.ValuesReady())
            {
                Console.WriteLine("values not initialized");
                Console.ReadLine();
                return;
            }
            Layer previousLayer = InputLayer;
            for (int HL = 0; HL < HiddenLayers.Length; HL++)
            {
                CalcLayer(HiddenLayers[HL], previousLayer);
                previousLayer = HiddenLayers[HL];
            }
            CalcLayer(OutputLayer, previousLayer);
        }

        private void CalcLayer(Layer layer, Layer prevLayer)
        {
            for (int neuronIndex = 0; neuronIndex < layer.NumOfNeurons; neuronIndex++)
            {
                double net = 0;
                for (int weightIndex = 0; weightIndex < layer.Neurons[neuronIndex].Weights.Length; weightIndex++)
                {
                    net += layer.Neurons[neuronIndex].Weights[weightIndex].Value * Convert.ToDouble(prevLayer.Neurons[weightIndex].Value);
                }
                layer.Neurons[neuronIndex].Value = net + layer.Neurons[neuronIndex].Bias.Value;
            }
        }

        public void ShowOutput()
        {
            for (int i = 0; i < OutputLayer.NumOfNeurons; i++)
            {
                Console.WriteLine("output " + i + ": " + OutputLayer.Neurons[i].Value);
            }
        }

        public void SlowLearn(TrainingData[] trainData)
        {
            do
            {
                for (int i = 0; i < trainData.Length; i++)
                {
                    InsertInput(trainData[i].Input);
                    FeedForward();
                    double error = CalcError(trainData[i].Output);
                    Mutate();
                    FeedForward();
                    double newError = CalcError(trainData[i].Output);
                    if (newError > error)
                    {
                        RevertMutation();
                    }
                }
            } while (!IsSmartEnough(trainData));
        }

        public void Mutate()
        {
            var ranIndex = Util.Ran.Next(0, NetworkData.Count);
            var dataWrapper = NetworkData[ranIndex];
            double ranValue = Util.Ran.Next(-10, 11) / 100.0;
            dataWrapper.Value = dataWrapper.Value + ranValue;
            LastMutation.RanIndex = ranIndex;
            LastMutation.RanValue = ranValue;
        }

        public void RevertMutation()
        {
            var dataWrapper = NetworkData[LastMutation.RanIndex];
            dataWrapper.Value = dataWrapper.Value - LastMutation.RanValue;
        }

        public bool IsSmartEnough(TrainingData[] trainData)
        {
            for (int i = 0; i < trainData.Length; i++)
            {
                InsertInput(trainData[i].Input);
                FeedForward();
                var output = GetOutput();
                for (int outIndex = 0; outIndex < output.Length; outIndex++)
                {
                    if (output[outIndex] > trainData[i].Output[outIndex] + 0.01 || output[outIndex] < trainData[i].Output[outIndex] - 0.01)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public double[] GetOutput()
        {
            var output = new double[OutputLayer.NumOfNeurons];
            for (int i = 0; i < OutputLayer.NumOfNeurons; i++)
            {
                output[i] = OutputLayer.Neurons[i].Value.Value;
            }
            return output;
        }

        public double CalcError(double[] expectedOutput)
        {
            double error = 0;
            double[] guessedOutput = GetOutput();
            for (int i = 0; i < expectedOutput.Length; i++)
            {
                error += Math.Abs(expectedOutput[i] - guessedOutput[i]);
            }
            return error;
        }

        public void TestIt(TrainingData[] trainData)
        {
            for (int i = 0; i < trainData.Length; i++)
            {
                InsertInput(trainData[i].Input);
                FeedForward();
                var guessedOutput = GetOutput();
                for (int j = 0; j < guessedOutput.Length; j++)
                {
                    Console.WriteLine("expected output" + j + ": " + trainData[i].Output[j] + " / actual output" + j + ": " + guessedOutput[j]);
                }
            }
        }
    }
}
