using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;

namespace digitTrainer
{
    public class NeuralNetwork
    {
        public Layer InputLayer { get; set; }
        public Layer OutputLayer { get; set; }
        [XmlArray("HiddenLayers")]
        [XmlArrayItem("Layer", typeof(Layer))]
        public Layer[] HiddenLayers { get; set; }
        public List<DataWrapper> NetworkData { get; set; } // weights and biases
        public Mutation LastMutation { get; set; }
        public double LearningRate { get; set; }
        public double HighestSuccesRate { get; set; }

        public NeuralNetwork()
        {

        }

        public NeuralNetwork(int numOfInputNeurons, int numOfOutputNeurons, int numOfHiddenLayers, int[] hiddenNeurons)
        {
            HighestSuccesRate = 0;
            LearningRate = 0.1;
            LastMutation = new Mutation();
            NetworkData = new List<DataWrapper>();
            InputLayer = new Layer(numOfInputNeurons);
            OutputLayer = new Layer(numOfOutputNeurons, hiddenNeurons[hiddenNeurons.Length - 1], NetworkData);
            HiddenLayers = new Layer[numOfHiddenLayers];
            HiddenLayers[0] = new Layer(hiddenNeurons[0], numOfInputNeurons, NetworkData);
            for (int i = 1; i < numOfHiddenLayers; i++)
            {
                HiddenLayers[i] = new Layer(hiddenNeurons[i], hiddenNeurons[i - 1], NetworkData);
            }
            SetNextLayers();
        }

        public void SetNextLayers()
        {
            InputLayer.nextLayer = HiddenLayers[0];
            for (int i = 0; i < HiddenLayers.Length - 1; i++)
            {
                HiddenLayers[i].nextLayer = HiddenLayers[i + 1];
            }
            HiddenLayers[HiddenLayers.Length - 1].nextLayer = OutputLayer;
            OutputLayer.nextLayer = null;
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
                CalcLayerForward(HiddenLayers[HL], previousLayer);
                previousLayer = HiddenLayers[HL];
            }
            CalcLayerForward(OutputLayer, previousLayer);
        }

        public void BackPropagation(TrainingData[] trainData)
        {
            do
            {
                for (int q = 0; q < 5; q++)
                {
                    int counter = 0;
                    for (int i = 0; i < trainData.Length; i++)
                    {
                        if (IsSmartEnough(new TrainingData[] { trainData[i] }))
                        {
                            counter++;
                            continue;
                        }
                        double[][] newOutputWeights = CalcWeightsBackwards(trainData[i].Output, HiddenLayers[HiddenLayers.Length - 1], OutputLayer);
                        double[][][] newHidLayWeights = new double[HiddenLayers.Length][][];
                        for (int k = HiddenLayers.Length - 1; k > 0; k--)
                        {
                            newHidLayWeights[k] = CalcWeightsBackwards(trainData[i].Output, HiddenLayers[k - 1], HiddenLayers[k]);
                        }
                        newHidLayWeights[0] = CalcWeightsBackwards(trainData[i].Output, InputLayer, HiddenLayers[0]);
                        double[] newOutputBiases = CalcBiasesBackwards(HiddenLayers[HiddenLayers.Length - 1], OutputLayer);
                        double[][] newHidLayBiases = new double[HiddenLayers.Length][];
                        newHidLayBiases[0] = CalcBiasesBackwards(InputLayer, HiddenLayers[0]);
                        for (int k = 1; k < HiddenLayers.Length; k++)
                        {
                            newHidLayBiases[k] = CalcBiasesBackwards(HiddenLayers[k - 1], HiddenLayers[k]);
                        }
                        for (int k = 0; k < OutputLayer.NumOfNeurons; k++)
                        {
                            for (int j = 0; j < OutputLayer.Neurons[k].Weights.Length; j++)
                            {
                                OutputLayer.Neurons[k].Weights[j].Value = newOutputWeights[k][j];
                            }
                        }
                        for (int l = 0; l < HiddenLayers.Length; l++)
                        {
                            for (int k = 0; k < HiddenLayers[l].NumOfNeurons; k++)
                            {
                                for (int j = 0; j < HiddenLayers[l].Neurons[k].Weights.Length; j++)
                                {
                                    HiddenLayers[l].Neurons[k].Weights[j].Value = newHidLayWeights[l][k][j];
                                }
                            }
                        }
                        for (int k = 0; k < OutputLayer.NumOfNeurons; k++)
                        {
                            OutputLayer.Neurons[k].Bias.Value = newOutputBiases[k];
                        }
                        for (int l = 0; l < HiddenLayers.Length; l++)
                        {
                            for (int k = 0; k < HiddenLayers[l].NumOfNeurons; k++)
                            {
                                HiddenLayers[l].Neurons[k].Bias.Value = newHidLayBiases[l][k];
                            }
                        }
                    }
                    if (counter / (double)trainData.Length > HighestSuccesRate)
                    {
                        HighestSuccesRate = counter / (double)trainData.Length;
                    }
                    Console.WriteLine("successfull to wrong answers: " + counter / (double)trainData.Length * 100 + "%   Highest succes: " + HighestSuccesRate * 100 + "%");
                }
                
                Util.SaveNetworkData(Program.NetworkDataPath, this);
                Console.WriteLine("saved");
            } while (!IsSmartEnough(trainData));
        }

        public void ShowWrongAnswer(TrainingData[] trainData)
        {
            for (int i = 0; i < trainData.Length; i++)
            {
                if (!IsSmartEnough(new TrainingData[] { trainData[i] }))
                {
                    double[] actualOutput = GetOutput();
                    Console.WriteLine();
                    Console.WriteLine("---WRONG ANSWER---");
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine("expected output" + j + ": " + trainData[i].Output[j] + "   actual output: " + actualOutput[j]);
                    }
                    DigitData.ShowDigit(trainData[i].Input);
                    Console.WriteLine("---WRONG ANSWER---");
                    Console.WriteLine();
                    return;
                }
            }
        }

        private double[][] CalcWeightsBackwards(double[] expectedOutput, Layer prevLayer, Layer currLayer)
        {
            double[][] newWeights = new double[currLayer.NumOfNeurons][];
            for (int layerNeuronI = 0; layerNeuronI < currLayer.Neurons.Length; layerNeuronI++)
            {
                newWeights[layerNeuronI] = new double[currLayer.Neurons[layerNeuronI].Weights.Length];
                currLayer.Neurons[layerNeuronI].TotalErrorToOutput = CalcErrorToNeuronValue(currLayer, layerNeuronI, expectedOutput);
                currLayer.Neurons[layerNeuronI].OutputToNet = currLayer.Neurons[layerNeuronI].Value.Value * (1 - currLayer.Neurons[layerNeuronI].Value.Value);
                for (int weightIndex = 0; weightIndex < currLayer.Neurons[layerNeuronI].Weights.Length; weightIndex++)
                {
                    double netToWeight = prevLayer.Neurons[weightIndex].Value.Value;
                    newWeights[layerNeuronI][weightIndex] = currLayer.Neurons[layerNeuronI].Weights[weightIndex].Value - LearningRate * currLayer.Neurons[layerNeuronI].TotalErrorToOutput * currLayer.Neurons[layerNeuronI].OutputToNet * netToWeight;
                }
            }
            return newWeights;
        }

        private double[] CalcBiasesBackwards(Layer prevLayer, Layer currLayer)
        {
            double[] newBiases = new double[currLayer.NumOfNeurons];
            for (int layerNeuronI = 0; layerNeuronI < currLayer.Neurons.Length; layerNeuronI++)
            {
                newBiases[layerNeuronI] = currLayer.Neurons[layerNeuronI].Bias.Value - LearningRate * currLayer.Neurons[layerNeuronI].TotalErrorToOutput * currLayer.Neurons[layerNeuronI].OutputToNet;
            }
            return newBiases;
        }

        private double CalcErrorToNeuronValue(Layer currLayer, int neuronIndex, double[] expectedOutput)
        {
            if (currLayer == OutputLayer)
            {
                if((expectedOutput[neuronIndex]==0.1 && currLayer.Neurons[neuronIndex].Value.Value>0.1) || (expectedOutput[neuronIndex] == 0.9 && currLayer.Neurons[neuronIndex].Value.Value < 0.9))
                {
                    return -(expectedOutput[neuronIndex] - currLayer.Neurons[neuronIndex].Value.Value);
                }
                return 0;
            }
            double errorToNeuronValue = 0;
            for (int i = 0; i < currLayer.nextLayer.NumOfNeurons; i++)
            {
                double totalErrorToOutput = currLayer.nextLayer.Neurons[i].TotalErrorToOutput;
                double outputToNet = currLayer.Neurons[i].OutputToNet;
                errorToNeuronValue += totalErrorToOutput * outputToNet * currLayer.nextLayer.Neurons[i].Weights[neuronIndex].Value;
            }
            return errorToNeuronValue;
        }

        private void CalcLayerForward(Layer layer, Layer prevLayer)
        {
            for (int neuronIndex = 0; neuronIndex < layer.NumOfNeurons; neuronIndex++)
            {
                double net = 0;
                for (int weightIndex = 0; weightIndex < layer.Neurons[neuronIndex].Weights.Length; weightIndex++)
                {
                    net += layer.Neurons[neuronIndex].Weights[weightIndex].Value * prevLayer.Neurons[weightIndex].Value.Value;
                }
                if (layer == OutputLayer)
                {
                    layer.Neurons[neuronIndex].Value = Util.Sigmoid(net + layer.Neurons[neuronIndex].Bias.Value);
                }
                else
                {
                    layer.Neurons[neuronIndex].Value = Util.Sigmoid(net + layer.Neurons[neuronIndex].Bias.Value);
                }
            }
        }

        public void ShowOutput()
        {
            double sum = 0;
            var dic = new Dictionary<double, string>();
            for (int j = 0; j < OutputLayer.NumOfNeurons; j++)
            {
                sum += OutputLayer.Neurons[j].Value.Value;
            }
            for (int i = 0; i < OutputLayer.NumOfNeurons; i++)
            {
                dic.Add(OutputLayer.Neurons[i].Value.Value / sum * 100,("output " + i + ": " + String.Format("{0:00}",OutputLayer.Neurons[i].Value/sum*100)+"%"));
            }
            var orderedPairs = dic.OrderByDescending(p => p.Key).ToList();
            var lines = orderedPairs.Select(p => p.Value).ToList();
            foreach (var item in lines)
            {
                Console.WriteLine(item);
            }
        }

        public void SlowLearn(TrainingData[] trainData)
        {
            do
            {
                for (int d = 0; d < 500; d++)
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
                }
                TestFew(trainData);
            } while (!IsSmartEnough(trainData));
        }

        public void TestFew(TrainingData[] trainData)
        {
            for (int i = 0; i < 10; i++)
            {
                TestOne(trainData[Util.Ran.Next(0, trainData.Length)]);
            }
        }

        public void Mutate()
        {
            var ranIndex = Util.Ran.Next(0, NetworkData.Count);
            var dataWrapper = NetworkData[ranIndex];
            double ranValue = Util.Ran.Next(-100, 101) / 100.0;
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
                    if (output[outIndex] > trainData[i].Output[outIndex] + 0.5 || output[outIndex] < trainData[i].Output[outIndex] - 0.5)
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
                if((expectedOutput[i]==0.1 && guessedOutput[i] > 0.1) || (expectedOutput[i]==0.9 && guessedOutput[i]<0.9))
                {
                    error += Math.Abs(expectedOutput[i] - guessedOutput[i]);
                }
            }
            return error;
        }

        public void TestOne(TrainingData trainData)
        {
            InsertInput(trainData.Input);
            FeedForward();
            var guessedOutput = GetOutput();
            for (int j = 0; j < guessedOutput.Length; j++)
            {
                Console.WriteLine("expected output" + j + ": " + trainData.Output[j] + " / actual output" + j + ": " + guessedOutput[j]);
            }
            Console.WriteLine();
        }

        public void TestAll(TrainingData[] trainData)
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
                DigitData.ShowDigit(trainData[i].Input);
                Console.WriteLine();
            }
        }

        public void ShowNetwork()
        {
            for (int i = 0; i < InputLayer.NumOfNeurons; i++)
            {
                Console.WriteLine("input value" + InputLayer.Neurons[i].Value);
            }
            for (int i = 0; i < HiddenLayers[0].NumOfNeurons; i++)
            {
                Console.WriteLine("hidden value" + HiddenLayers[0].Neurons[i].Value);
                for (int j = 0; j < HiddenLayers[0].Neurons[i].Weights.Length; j++)
                {
                    Console.WriteLine("weight" + HiddenLayers[0].Neurons[i].Weights[j].Value);
                }

            }
            for (int i = 0; i < OutputLayer.NumOfNeurons; i++)
            {
                Console.WriteLine("output value" + OutputLayer.Neurons[i].Value);
                for (int j = 0; j < OutputLayer.Neurons[i].Weights.Length; j++)
                {
                    Console.WriteLine("weight" + OutputLayer.Neurons[i].Weights[j].Value);
                }

            }
        }
    }
}
