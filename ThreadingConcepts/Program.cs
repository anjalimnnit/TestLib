using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingConcepts

{
    class Program
    {
        private static AutoResetEvent event_1 = new AutoResetEvent(true);
        private static AutoResetEvent event_2 = new AutoResetEvent(false);
        static void Main(string[] args)
        {

            var t1 = Task.Factory.StartNew(() => PrintOddNumbers());
            var t2 = Task.Factory.StartNew(() => PrintEvenNumbers());
         
            Task.WaitAll(t1, t2);
            Console.WriteLine("End");
            Console.ReadLine();
        }

        static void PrintOddNumbers()
        {
            int[] arr = new int[] { 1, 3, 5, 7, 9, 11, 13, 15 };
            foreach (var item in arr)
            {
                event_1.WaitOne();
                Console.WriteLine(item);
                event_2.Set();
            }
        }

        static void PrintEvenNumbers()
        {
            int[] arr = new int[] { 2, 4, 6, 8, 10, 12, 14 };
            foreach (var item in arr)
            {
                event_2.WaitOne();
                Console.WriteLine(item);
                event_1.Set();
            }
        }
    }
}