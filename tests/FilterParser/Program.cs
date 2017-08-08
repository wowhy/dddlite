using System;
using System.Diagnostics;
using DDDLite.Domain;
using DDDLite.Specifications;

namespace FilterParser
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var count = 100000;
            var watch = new Stopwatch();
            watch.Start();

            Console.Write("0%");
            var percent = 0;
            for (var i = 0; i < count; i++)
            {
                var test = new DDDLite.WebApi.Internal.Parser.FilterParser<Person>();
                var spec = test.Parse($"name != null && name.Contains(\"{i}\")");

                if ((int)(i / (double)count * 100) > percent)
                {
                    percent = (int)(i / (double)count * 100);
                    Console.Write("\b\b\b");
                    Console.Write($"{percent}%");
                }
            }

            Console.Write("\b\b\b");
            Console.Write($"100%\r\n");

            watch.Stop();

            Console.WriteLine("Total: {0}ms, Avg: {1}ms", watch.ElapsedMilliseconds, watch.ElapsedMilliseconds / (double)count);
        }
    }
}
