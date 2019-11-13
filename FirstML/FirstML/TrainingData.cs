using System;
using System.Collections.Generic;
using System.Text;

namespace FirstML
{
    public class TrainingData
    {
        public float?[] Input { get; set; }
        public float? Output { get; set; }

        public TrainingData(float? input1, float? input2, float? output1)
        {
            Input = new float?[] { input1, input2 };
            Output = output1;
        }
    }
}
