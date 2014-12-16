using System.Collections.Concurrent;

namespace Playground
{
    using System;
    using System.Threading.Tasks;
    using StructureMap;
    using StructureMap.Pipeline;

    class Program
    {
        static void Main(string[] args)
        {
            Go();
            Console.ReadLine();
        }

        private static void Go()
        {
            var container = new Container(
                c =>
                    {
                        c.For<Foo>().Use(() => new Foo("singleton")).Named("singleton").Singleton();
                        c.For<Foo>()
                            .Use(() => new Foo("thread"))
                            .Named("thread")
                            .SetLifecycleTo(new ThreadLocalStorageLifecycle());
                        c.For<Foo>().Use(() => new Foo("transient")).Named("transient").SetLifecycleTo(new TransientLifecycle());
                    });

            container.GetInstance<Foo>("singleton").DoWork("container");
            container.GetInstance<Foo>("thread").DoWork("container");
            container.GetInstance<Foo>("transient").DoWork("container");
            Console.WriteLine();

            using (var nested = container.GetNestedContainer())
            {
                nested.GetInstance<Foo>("singleton").DoWork("nested");
                nested.GetInstance<Foo>("thread").DoWork("nested");
                nested.GetInstance<Foo>("transient").DoWork("nested");
                Console.WriteLine();

                Task.Factory.StartNew(
                    () =>
                        {
                            nested.GetInstance<Foo>("singleton").DoWork("nested task");
                            nested.GetInstance<Foo>("thread").DoWork("nested task");
                            nested.GetInstance<Foo>("transient").DoWork("nested task");
                        }).Wait();
            }
        }

        class Foo : IDisposable
        {
            private static readonly ConcurrentDictionary<string, int> Increment = new ConcurrentDictionary<string, int>();

            private readonly string _name;
            private readonly int _increment;

            public Foo(string name)
            {
                this._name = name;
                this._increment = Increment.AddOrUpdate(name, n => 1, (s, i) => i + 1);
            }

            public void DoWork(string s)
            {
                Console.WriteLine("{0} {1}:{2}", s, this._name, this._increment);
            }

            public void Dispose()
            {
                Console.WriteLine("Disposed {0}:{1}", this._name, this._increment);
            }
        }
    }
}
