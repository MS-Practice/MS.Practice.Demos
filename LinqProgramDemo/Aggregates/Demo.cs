using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqProgramDemo.Aggregates
{
    class Demo
    {
        public static void Part1()
        {
            var ret = Enumerable.Range(1, 100)
                .Aggregate((a, b) => a + b);
            Console.WriteLine(ret);
        }

        public static void Part2()
        {
            var ret = Enumerable.Range(1, 5)
                .Aggregate(Acculate);
            Console.WriteLine(ret);
        }

        public static void Part3()
        {
            List<Bar> bars = new List<Bar> {
                new Bar{ Age = 24,ID=1,Man = true,Name = "Marson",Fars = new List<Far>()}
            };
            var bar = bars.Aggregate(GetBar);
        }

        private static int Acculate(int a, int b)
        {
            return a * b;
        }

        private static Bar GetBar(Bar a, Bar b)
        {

            var bar = new Bar();
            bar.Fars = new List<Far>(a.Fars.Count + b.Fars.Count);
            bar.Age = a.Age + b.Age;
            bar.Fars.AddRange(a.Fars);
            bar.Fars.AddRange(b.Fars);
            bar.Man = a.Man;
            bar.Name = a.Name + b.Name;
            return bar;
        }
    }

    class Bar
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Man { get; set; }
        public int Age { get; set; }
        public List<Far> Fars { get; set; }
    }

    class Far
    {
        public int ID { get; set; }
        public string CloseType { get; set; }
        public string CloseName { get; set; }
    }
}
