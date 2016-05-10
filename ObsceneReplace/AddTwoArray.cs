using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ObsceneReplace
{
    public class AddTwoArray
    {
        private static int SumA(int[,] array, int n)
        {
            int sum = 0;
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++) {
                    sum += array[x, y];
                }
            }
            return sum;
        }

        private static int SumB(int[,] array, int n)
        {
            int sum = 0;
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    sum += array[x, y]; 
                }
            }
            return sum;
        }
        private static int ParallelSumA(int[,] array, int n)
        {
            int processorCount = Environment.ProcessorCount;
            int[] result = new int[processorCount];

            Parallel.For(0, processorCount, (part) => {
                int partsum = 0;
                int minInclusive = part * n / processorCount;
                int maxExclusive = minInclusive + n / processorCount;
                for (int x = minInclusive; x < maxExclusive; x++)
                {
                    for (int y = 0; y < n; y++)
                    {
                        //result[part] += array[x, y];    //
                        partsum += array[x, y];
                    }
                }
                result[part] = partsum;
            });
            int sum = 0;
            for (int i = 0; i < result.Length; i++)
            {
                sum += result[i];
            }
            return sum;
        }

        public static void TestLocality(int[,] array, int n)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            for (int i = 0; i < 100; i++)
                ParallelSumA(array, n);
            watch1.Stop();
            Console.WriteLine("ParallelSumA: " + watch1.Elapsed);

            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            for (int i = 0; i < 100; i++)
                SumA(array, n);
            watch2.Stop();
            Console.WriteLine("SumA: " + watch2.Elapsed);

            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            for (int i = 0; i < 100; i++)
                SumB(array, n);
            watch3.Stop();
            Console.WriteLine("SumB: " + watch3.Elapsed);

        }
    }
}
