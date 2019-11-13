using System;
using System.Collections.Generic;
using System.Text;

namespace FirstML
{
    public class Layer
    {
        public Neuron[] Neurons { get; set; }

        public Layer(Neuron[] neurons)
        {
            Neurons = neurons;
        }
    }
}
