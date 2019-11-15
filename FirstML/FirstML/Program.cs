using System;

namespace FirstML
{
    public class Program
    {
        public static Random Ran { get; set; } = new Random();

        static void Main(string[] args)
        {
            ML.Init();
            Console.WriteLine("stupid brain");
            ML.Show();
            for (int i = 0; i < ML.Inputs.Length; i++)
            {
                ML.InsertInput(ML.Inputs[i]);
                ML.Proccess();
                ML.ShowResults(i);
            }
            Console.ReadLine();
            do
            {
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < ML.Inputs.Length; j++)
                    {
                        ML.Mutate(j);
                    }
                }
            } while (!ML.IsSmart());

            Console.WriteLine("trained brain");
            for (int i = 0; i < ML.Inputs.Length; i++)
            {
                ML.InsertInput(ML.Inputs[i]);
                ML.Proccess();
                ML.ShowResults(i);
            }
            ML.Show();
            do
            {
                double input1 = Convert.ToDouble(Console.ReadLine());
                double input2 = Convert.ToDouble(Console.ReadLine());
                ML.InsertInput(input1, input2);
                ML.Proccess();
                Console.WriteLine("Guess: "+ML.OutputLayer.Neurons[0].Value);
            } while (true);
        }
    }
}
