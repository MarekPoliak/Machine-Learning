using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace digitTrainer
{
    public class Layer
    {
        public Neuron[] Neurons { get; set; }
        public int NumOfNeurons { get; set; }
        [XmlIgnore]
        public Layer nextLayer { get; set; }

        public Layer()
        {

        }

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
