using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Playground
{

    internal class RecreateTimerProgram
    {
        private Timer _timer;

        public void Start()
        {
            InitTimer(1000);
        }

        public void Stop()
        {
            _timer.Stop();
            Console.WriteLine("Timer stopped");
        }

        private void InitTimer(double interval)
        {
            _timer = new Timer
                         {
                             AutoReset = true,
                             Interval = interval
                         };

            _timer.Elapsed += OnElapsed;
            _timer.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Dispose();
            InitTimer(1500);

            Console.WriteLine("Elapsed on thread {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
        }
    }
}