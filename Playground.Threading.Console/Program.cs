namespace Playground
{
    using System;
    using System.Diagnostics;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            //new CancellationProgram().Start();
            var p = new ChangeTimerIntervalProgram();
            //var p = new RecreateTimerProgram();
            p.Start();

            Console.ReadLine();
            p.Stop();

            Console.WriteLine("*************************");
            Console.WriteLine("Elapsed time: {0}", watch.Elapsed);
            Console.WriteLine("");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }
    }
}