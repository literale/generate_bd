using System;
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
            string[] txts_for_mix = { "cat_food_for.txt", "cat_food_material.txt" };
            Generate_product_universal gpu_cat_food_dry = new Generate_product_universal("cat_food_brands.txt", "cat_food_type_food.txt", txts_for_mix, 30, 100, "cat_food_dry", "pets_goods", UNIT.GRAM, 10);
            gpu_cat_food_dry.GO(100);
            Console.ReadKey();
        }
        
    }
    
}
