using System;
using System.Collections.Generic;
using System.Text;

namespace FirstML
{
    public class Neuron
    {
        public float?[] Weights { get; set; }
        public float? Bias { get; set; }
        public float? Value { get; set; }
        public float? Net { get; set; }

        public Neuron(int numOfWeights, float? value)
        {
            Weights = new float?[numOfWeights];
            for (int i = 0; i < numOfWeights; i++)
            {
                Weights[i] = 1;
            }
            Bias = 1;
            Value = value;
        }
    }
}
