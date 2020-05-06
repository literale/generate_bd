using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Console_product_generation
{
    class Check
    {
        private Random r = new Random();
        private DateTime check_date;
        private int ID_customer = 0;
        private double total_sum = 0;
        private double product_total = 0;
        private List<Product> products = new List<Product>();
        public double Generate_check(int ID_shop, int ID_check, int prod_count)
        {
            double amount = 0;
            return amount; 
        }

        //static List<int> check_type = new List<int>();
        //static List<int> check_size = new List<int>();
        //public static void PrinCheck()
        //{
        //    double avType = 0;
        //    double avSize = 0;
        //    Console.WriteLine("РАЗМЕРЫ ЧЕКОВ");
        //    for (int i = 0; i < check_type.Count; i++)
        //    {
        //        Console.WriteLine("Тип чека: "+ check_type[i] + " ; Товаров в чеке: " + check_size[i]);
        //        avSize += check_size[i];
        //        avType += check_type[i];
        //    }
        //    Console.WriteLine("Средний тип чека = " + avType / check_type.Count + " ; Средний размер чека = " + avSize / check_size.Count);
        //}

        public int check_count(int size, Random r, Math_help mh)
        {
          //  check_type.Add(size);

            int count = 1;
            switch (size)
            {
                case 1:
                    {
                        count = mh.SampleGaussian(r, 3, 10, 1, 5);
                        //Console.WriteLine(count);
                        //check_size.Add(count);
                        break;
                    }
                case 2:
                    {
                        count = mh.SampleGaussian(r, 5, 10, 1, 10);
                        //Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 3:
                    {
                        count = mh.SampleGaussian(r, 7, 10, 4, 10);
                        //Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 4:
                    {
                        count = mh.SampleGaussian(r, 10, 10, 4, 15);
                        //Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 5:
                    {
                        count = mh.SampleGaussian(r, 10, 10, 8, 18);
                        //Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 6:
                    {
                        count = mh.SampleGaussian(r, 15, 10, 10, 25);
                        //Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 7:
                    {
                        count = mh.SampleGaussian(r, 20, 10, 10, 25);
                       // Console.WriteLine(count);
                      //  check_size.Add(count);
                        break;
                    }
                case 8:
                    {
                        count = mh.SampleGaussian(r, 20, 15, 15, 30);
                        //Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 9:
                    {
                        count = mh.SampleGaussian(r, 25, 10, 15, 30);
                       // Console.WriteLine(count);
                       // check_size.Add(count);
                        break;
                    }
                case 10:
                    {
                        count = mh.SampleGaussian(r, 30, 10, 20, 50);
                       // Console.WriteLine(count);
                      //  check_size.Add(count);
                        break;
                    }
                case 11:
                    {
                        count = mh.SampleGaussian(r, 40, 10, 20, 50);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 12:
                    {
                        count = mh.SampleGaussian(r, 40, 10, 30, 70);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
               
                case 13:
                    {
                        count = mh.SampleGaussian(r,55, 10, 30, 70);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 14:
                    {
                        count = mh.SampleGaussian(r, 60, 10, 40, 100);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 15:
                    {
                        count = mh.SampleGaussian(r, 80, 10, 40, 100);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 16:
                    {
                        count = mh.SampleGaussian(r, 85, 10, 50, 125);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 17:
                    {
                        count = mh.SampleGaussian(r, 95, 10, 70, 125);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 18:
                    {
                        count = mh.SampleGaussian(r, 110, 10, 70, 150);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 19:
                    {
                        count = mh.SampleGaussian(r, 130, 10, 100, 150);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                case 20:
                    {
                        count = mh.SampleGaussian(r, 180, 10, 150, 200);
                        // Console.WriteLine(count);
                        //  check_size.Add(count);
                        break;
                    }
                default:
                    {
                        count = r.Next(1, 200);
                        break;
                    }

            }
            return count;
        }
    }
}
