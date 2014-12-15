namespace TplPlayground.Console
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class CancellationProgram
    {
        public void Run()
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            Task task = Task.Factory.StartNew(() => DoWork(token), token);
            Task next = task.ContinueWith(t => Console.WriteLine("Finished!"));

            if (Console.ReadKey().KeyChar == 'q')
            {
                Console.WriteLine();
                source.Cancel();
            }
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                PrintDetails(ex);
            }

            next.Wait();
        }

        private static void DoWork(CancellationToken token)
        {
            int timerCounter = 0;
            using (var timer = new System.Timers.Timer(1000))
            {
                timer.Elapsed += (sender, args) => Console.WriteLine("Times " + timerCounter++);
                timer.Start();

                int counter = 0;
                while (true)
                {
                    Task.Delay(1000, token).Wait();
                    Console.WriteLine("Running " + counter++);
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
        }

        private void PrintDetails(AggregateException aggregateException)
        {
            foreach (var innerEx in aggregateException.InnerExceptions)
            {
                var canceledException = innerEx as TaskCanceledException;
                if (canceledException != null)
                {
                    Console.WriteLine("Task {0} was canceled ", canceledException.Task.Id);
                }
                var innerAggregatedException = innerEx as AggregateException;
                if (innerAggregatedException != null)
                {
                    PrintDetails(innerAggregatedException);
                }
            }
        }
    }
}