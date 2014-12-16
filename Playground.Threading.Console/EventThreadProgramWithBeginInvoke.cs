namespace Playground
{
    using System;
    using System.Linq;
    using System.Threading;

    public sealed class EventThreadProgramWithBeginInvoke
    {
        public void Run()
        {
            Console.WriteLine("Program running on thread {0}", Thread.CurrentThread.ManagedThreadId);

            Foo[] foos = Enumerable.Range(0, 10).Select(
                i =>
                {
                    Thread.Sleep(100);
                    var foo = new Foo(i);
                    foo.OnChanged += this.FooChanged;
                    Console.WriteLine("Created {0} running on thread {1}", foo.Number, Thread.CurrentThread.ManagedThreadId);
                    return foo;
                }).ToArray();

            foreach (var foo in foos)
            {
                foo.Change();
            }
        }

        private void FooChanged(object sender, Foo.ChangedStateEventArgs changedStateEventArgs)
        {
            Thread.Sleep(100);
            var foo = (Foo)sender;
            Console.WriteLine("Handler for {0} running on thread {1}", foo.Number, Thread.CurrentThread.ManagedThreadId);
        }

        public sealed class Foo
        {
            public int Number { get; private set; }

            public Foo(int number)
            {
                this.Number = number;
            }

            public event EventHandler<ChangedStateEventArgs> OnChanged;

            public void Change()
            {
                var handler = this.OnChanged;
                if (handler != null)
                {
                    //Console.WriteLine("Event for {0} triggered on thread {1}", Number, Thread.CurrentThread.ManagedThreadId);
                    handler(this, new ChangedStateEventArgs());
                    handler.BeginInvoke(this, new ChangedStateEventArgs(), this.EndAsyncEvent, null);
                }
            }

            private void EndAsyncEvent(IAsyncResult ar)
            {
                //Console.WriteLine("End async event for {0} triggered on thread {1}", Number, Thread.CurrentThread.ManagedThreadId);
            }

            public sealed class ChangedStateEventArgs
            {
            }
        }
    }
}
