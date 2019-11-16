using System;
using System.Collections.Generic;
using System.Text;

namespace RecognitionOfHandWriting
{
    class Layer
    {
        public Neuron[] Neurons { get; set; }
        public int NumOfNeurons { get; set; }

        public Layer(int numOfNeurons, int numOfPrevNeurons, List<DataWrapper> networkData)
        {
            Neurons = new Neuron[numOfNeurons];
            for (int i = 0; i < numOfNeurons; i++)
            {
                Neurons[i] = new Neuron(numOfPrevNeurons, networkData);
            }
            NumOfNeurons = numOfNeurons;
        }

        public Layer(int numOfNeurons)
        {
            Neurons = new Neuron[numOfNeurons];
            for (int i = 0; i < numOfNeurons; i++)
            {
                Neurons[i] = new Neuron();
            }
            NumOfNeurons = numOfNeurons;
        }

        public bool ValuesReady()
        {
            foreach (var neuron in Neurons)
            {
                if (!neuron.Value.HasValue)
                    return false;
            }
            return true;
        }
    }
}
