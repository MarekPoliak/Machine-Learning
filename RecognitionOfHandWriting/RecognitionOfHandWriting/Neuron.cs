using System;
using System.Collections.Generic;
using System.Text;

namespace RecognitionOfHandWriting
{
    class Neuron
    {
        public DataWrapper[] Weights { get; set; }
        public double? Value { get; set; }
        public DataWrapper Bias { get; set; }

        public Neuron(int numOfWeights, List<DataWrapper> networkData)
        {
            Weights = new DataWrapper[numOfWeights];
            for (int i = 0; i < numOfWeights; i++)
            {
                Weights[i] = new DataWrapper(1);
                networkData.Add(Weights[i]);
            }
            Bias = new DataWrapper(1);
            networkData.Add(Bias);
        }

        public Neuron()
        {
        }
    }
}
