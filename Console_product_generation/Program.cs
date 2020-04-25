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
            Console.WriteLine("Начата генерация магазинов");
            Read_and_generate.Generate_shops();
            Console.WriteLine("Закончена генерация магазинов");
            Console.WriteLine("Начата генерация продуктов");
            Read_and_generate.Generate_products();
            Console.WriteLine("Закончена генерация продуктов");
            Console.WriteLine("Начата генерация людей");
            Read_and_generate.Generate_people(true, 5000);
            Console.WriteLine("Закончена генерация людей");
            Console.WriteLine("DONE");
            Console.ReadKey();
        }
        
    }
    
}
