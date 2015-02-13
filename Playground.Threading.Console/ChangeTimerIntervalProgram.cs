using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Playground
{

    internal class ChangeTimerIntervalProgram
    {
        private Timer _timer;

        public void Start()
        {
            _timer = new Timer
            {
                AutoReset = true,
                Interval = 1000
            };

            _timer.Elapsed += OnElapsed;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            Console.WriteLine("Timer stopped");
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Interval = 1500;
            Console.WriteLine("Elapsed on thread {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
        }
    }
}