using System;
using System.Diagnostics;
using DDDLite.Domain;
using DDDLite.Specifications;

namespace FilterParser
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }

        public bool Checked { get; set; }

        public int Age { get; set; }

        public decimal Money1 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // TestOne();
            Benchmark();
        }

        private static void TestOne()
        {
            // var test = new DDDLite.WebApi.Internal.Parser.FilterParser<Person>();
            // var spec = test.Parse($"name ne null or name ne '123'");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"123 ne age");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"money1 lt 3.1415926");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"not checked");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"not false");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"not (name ne null or age eq 18)");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"name ne null and name ne '123' or age gt 10 or age lt 99");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"name ne null and (name ne '123' or age gt 10) or age lt 99");
            // Console.WriteLine(spec.Expression);

            // spec = test.Parse($"name ne null and not (name ne '123' or age gt 10) or age lt 99");
            // Console.WriteLine(spec.Expression);
        }

        private static void Benchmark()
        {
            var count = 1000000;
            var stop = new Stopwatch();
            stop.Start();

            for (int i = 0; i < count; i++)
            {
                var test = new DDDLite.WebApi.Internal.Parser.FilterParser<Person>("name ne null or name ne '123'").Parse();
            }

            var avg = (double)stop.ElapsedMilliseconds / count;
            Console.WriteLine($"{count} times taked {stop.ElapsedMilliseconds} ms, 1 time {avg} ms");
        }
    }
}
