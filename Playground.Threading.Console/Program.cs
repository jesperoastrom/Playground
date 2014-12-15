namespace TplPlayground.Console
{
    using System;
    using System.Diagnostics;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            new CancellationProgram().Run();

            Console.WriteLine("*************************");
            Console.WriteLine("Elapsed time: {0}", watch.Elapsed);
            Console.WriteLine("");
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }
    }
}