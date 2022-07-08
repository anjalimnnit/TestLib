using System;

namespace GarbageCollection
{
    public enum ResourceState
    {
        FREE, BUSY
    }

    public class Resource
    {
        public static readonly Resource Instance = new Resource();
        private Resource() { }
        public ResourceState State = ResourceState.FREE;

    }
    public class A
    {

        public static System.Threading.AutoResetEvent _handle = new System.Threading.AutoResetEvent(true);
        public A()
        {
            lock (_handle)
            {
                if (Resource.Instance.State == ResourceState.FREE)
                {
                    Console.WriteLine($"Resource Owned By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }
                else
                {
                    Console.WriteLine($"Resource Awaited By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    //wait
                    _handle.WaitOne();
                    Console.WriteLine($"Resource Owned By {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                }
                Resource.Instance.State = ResourceState.BUSY;
            }
        }

        public void UseResource()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Resource Used By thread {System.Threading.Thread.CurrentThread.ManagedThreadId} {i} times");
                System.Threading.Thread.Sleep(1000);
            }

        }
    }

    class _Program
    {
        static void Main()
        {
            new System.Threading.Thread(Client).Start();
            new System.Threading.Thread(Client).Start();
        }
        static void Client()
        {
            A obj = null;
            obj = new A();
            obj.UseResource();
            obj = null;
            GC.Collect();
            Console.WriteLine("Successfully Garbage collected object A");
        }
    }
}