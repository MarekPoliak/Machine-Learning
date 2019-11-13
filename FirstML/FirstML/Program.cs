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
            for (int i = 0; i < 4; i++)
            {
                ML.InsertInput(ML.Inputs[i]);
                ML.Proccess();
                ML.ShowResult();
            }
            for (int i = 0; i < 100000; i++)
            {
                ML.Mutate(Ran.Next(0,4));
            }
            Console.WriteLine("trained brain");
            for (int i = 0; i < 4; i++)
            {
                ML.InsertInput(ML.Inputs[i]);
                ML.Proccess();
                ML.ShowResult();
            }
        }
    }
}
