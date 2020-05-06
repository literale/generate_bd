using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using MySql.Data.MySqlClient;

namespace Console_product_generation
{
    enum UNIT { GRAM, MILILITRE, PIECE }
    class Program
    {
        static void Main(string[] args)
        {
            SQL_Commands.setConnection(Info.connection_string);
            //Read_and_generate.Random_country();
            //Console.WriteLine("Начата генерация магазинов");
            //Read_and_generate.Generate_shops();
            //Console.WriteLine("Закончена генерация магазинов");
            //Console.WriteLine("Начата генерация продуктов");
            //Read_and_generate.Generate_products();
            //Console.WriteLine("Закончена генерация продуктов");
            //Console.WriteLine("Начата генерация людей");
            //Read_and_generate.Generate_people(false, 1500);
            //Console.WriteLine("Закончена генерация людей");
            Read_and_generate.Generate_check(100);
            //Console.WriteLine("DONE");
            // Math_help mh = new Math_help();
            // Random r = new Random();
            // for (int i = 0; i < 100; i++)
            // {

            //     mh.SampleGaussian(r, 4, 10, 1, 10);
            // }
            //mh.PrinList();
            Console.ReadKey();
        }
        
    }
    
}
