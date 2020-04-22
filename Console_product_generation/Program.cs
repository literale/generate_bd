using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Console_product_generation
{
    enum UNIT { GRAM, MILILITRE, PIECE }
    class Program
    {
        static void Main(string[] args)
        {
            SQL_Commands.setConnection(Info.connection_string);
            //string[] txts_for_mix = { "cat_food_for.txt", "cat_food_material.txt" };
            //Generate_product_universal gpu_cat_food_dry = new Generate_product_universal("cat_food_brands.txt", "cat_food_type_food.txt", txts_for_mix, 30, 100, "cat_food_dry", "pets_goods", UNIT.GRAM, 10);
            //gpu_cat_food_dry.GO(100);
            string path = AppDomain.CurrentDomain.BaseDirectory + "gen";
            Console.WriteLine(path);
            DirectoryInfo di = new DirectoryInfo(path);
            List<String> txts_for_mix = new List<String>();
            string brand_txt = "";
            string weight_txt = "";
            string[] info;
            UNIT u = UNIT.GRAM;
            int cost_min = 0;
            int cost_max = 0;
            int vague = 0;
            foreach (DirectoryInfo big_type in di.GetDirectories())
            {
                Console.WriteLine(big_type.Name);
                foreach (DirectoryInfo little_type in big_type.GetDirectories())
                {
                    txts_for_mix = new List<String>();
                    Console.WriteLine(little_type.Name);
                    foreach (DirectoryInfo dir in little_type.GetDirectories())
                    {
                        if (dir.Name.Equals("brand"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            brand_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                            Console.WriteLine(brand_txt);
                        }
                        if (dir.Name.Equals("mix"))
                        {     
                            FileInfo[] files = dir.GetFiles();
                            foreach (FileInfo f in files)
                            {
                                txts_for_mix.Add(path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + f.Name);
                                Console.WriteLine(txts_for_mix.Last());
                            }
                        }
                        if (dir.Name.Equals("weight"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            weight_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                            Console.WriteLine(weight_txt);
                        }
                        if (dir.Name.Equals("info"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            string info_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                            Console.WriteLine(info_txt);
                            info = File.ReadAllLines(info_txt);
                            foreach(string i in info)
                            {
                                string[] line = i.Split(' ');
                                if (line[0].Equals("UNIT"))
                                {
                                    if (line[2].ToUpper().Equals("GRAM"))
                                        {
                                        u = UNIT.GRAM;
                                        }
                                    else if (line[2].ToUpper().Equals("MILILITRE"))
                                    {
                                        u = UNIT.MILILITRE;
                                    }
                                    else
                                    { u = UNIT.PIECE; }
                                }
                                else if (line[0].Equals("cost_min"))
                                { cost_min = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("cost_max"))
                                { cost_max = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("vague"))
                                { vague = Convert.ToInt32(line[2]); }
                                
                            }
                            Console.WriteLine(u.ToString() + " " + cost_min + " " + cost_max + " " + vague);
                        }
                    }
                    Generate_product_universal gpu_cat_food_dry = new Generate_product_universal(brand_txt, weight_txt, txts_for_mix.ToArray(), cost_min, cost_max, little_type.Name, big_type.Name, u, vague);
                    gpu_cat_food_dry.GO(100);

                }
                //    list_dirs.Add(dir.Name);
                //Console.WriteLine(dir.Name);
            }
            Console.ReadKey();
        }
        
    }
    
}
