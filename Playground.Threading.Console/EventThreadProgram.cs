namespace Playground
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class EventThreadProgram
    {
        public void Run()
        {
            Console.WriteLine("Program running on thread {0}", Thread.CurrentThread.ManagedThreadId);

            Task<Foo>[] tasks = Enumerable.Range(1, 10).Select(i => Task<Foo>.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                var foo = new Foo(i);
                foo.OnChanged += this.FooChanged;
                Console.WriteLine("Created {0} running on thread {1}", foo.Number, Thread.CurrentThread.ManagedThreadId);
                return foo;
            })).ToArray();

            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                task.Result.Change();
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
                }
            }

            public sealed class ChangedStateEventArgs
            {
            }
        }
    }
}
