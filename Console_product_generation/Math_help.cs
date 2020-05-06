using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_product_generation
{
    class Math_help
    {

        private List<int> l;
        public Math_help()
        {
            l = new List<int>();
        }
        public int SampleGaussian(Random random, double mean, double stddev, int min, int max)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            double a =  y1 * stddev + mean;
            a = a - 1;
            if (a<min || a > max)
            {
                a = SampleGaussian(random, mean, stddev,  min, max);
            }
            else l.Add(Convert.ToInt32(a));
            return Convert.ToInt32(a);
        }

        public void PrinList()
        {
            double av = 0;
            l.Sort();
            foreach(int i in l)
            {
                Console.Write(i+" " );
                av += i;
            }
            Console.WriteLine("AV = " + av / l.Count);
        }



    }
}
