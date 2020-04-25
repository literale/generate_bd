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
            Read_and_generate.Generate_shops();
            Read_and_generate.Generate_products();
            Read_and_generate.Generate_people(true, 50);

            Console.WriteLine("DONE");
            Console.ReadKey();
        }
        
    }
    
}
