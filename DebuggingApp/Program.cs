﻿using System;
using System.Threading;

namespace DebuggingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Hello World!");
                Thread.Sleep(1000);
            }
        }
    }
}
