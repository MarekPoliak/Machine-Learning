using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace digitTrainer
{
    public class Neuron
    {
        [XmlArray("weights")]
        [XmlArrayItem("Datawrapper", typeof(DataWrapper))]
        public DataWrapper[] Weights { get; set; }
        public double? Value { get; set; }
        public DataWrapper Bias { get; set; }
        public double TotalErrorToOutput { get; set; }
        public double OutputToNet { get; set; }

        public Neuron()
        {

        }

        public Neuron(int numOfWeights, List<DataWrapper> networkData)
        {
            Weights = new DataWrapper[numOfWeights];
            for (int i = 0; i < numOfWeights; i++)
            {
                Weights[i] = new DataWrapper(Util.GetRan());
                networkData.Add(Weights[i]);
            }
            Bias = new DataWrapper(Util.GetRan());
            networkData.Add(Bias);
        }

        public Neuron(DataWrapper[] weights, double value, DataWrapper bias)
        {
            Weights = weights;
            Value = value;
            Bias = bias;
        }
    }
}
