using System;

namespace FlipFlop.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Features.Initialize();

            while (true)
            {
                try
                {
                    Console.WriteLine("Show users" + Features.IsEnabled("Show users"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.ReadKey();
            }
        }
    }
}
