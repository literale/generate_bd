using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_product_generation
{
    class Read_and_generate
    {

        public static void Generate_products()
        {
            
            string path = AppDomain.CurrentDomain.BaseDirectory + "gen" + "\\" + "products";
            //Console.WriteLine(path);
            DirectoryInfo di = new DirectoryInfo(path);
            List<String> txts_for_mix = new List<String>();
            string brand_txt = "";
            string weight_txt = "";
            string[] info;
            UNIT u = UNIT.GRAM;
            int price_min = 0;
            int price_max = 0;
            int count = 0;
            int price_rejection = 0;
            int pers_have = 0;
            int amount = 0;
            foreach (DirectoryInfo big_type in di.GetDirectories())
            {
               // Console.WriteLine(big_type.Name);
                foreach (DirectoryInfo little_type in big_type.GetDirectories())
                {
                    txts_for_mix = new List<String>();
                  //  Console.WriteLine(little_type.Name);
                    foreach (DirectoryInfo dir in little_type.GetDirectories())
                    {
                        if (dir.Name.Equals("brand"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            brand_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                            //Console.WriteLine(brand_txt);
                        }
                        if (dir.Name.Equals("mix"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            foreach (FileInfo f in files)
                            {
                                txts_for_mix.Add(path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + f.Name);
                                //Console.WriteLine(txts_for_mix.Last());
                            }
                        }
                        if (dir.Name.Equals("weight"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            weight_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                           // Console.WriteLine(weight_txt);
                        }
                        if (dir.Name.Equals("info"))
                        {
                            FileInfo[] files = dir.GetFiles();
                            string info_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                           // Console.WriteLine(info_txt);
                            info = File.ReadAllLines(info_txt);
                            foreach (string i in info)
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
                                else if (line[0].Equals("price_min"))
                                { price_min = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("price_max"))
                                { price_max = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("count"))
                                { count = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("price_rejection"))
                                { price_rejection = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("pers_have"))
                                { pers_have = Convert.ToInt32(line[2]); }
                                else if (line[0].Equals("amount"))
                                { amount = Convert.ToInt32(line[2]); }


                            }
                           // Console.WriteLine(u.ToString() + " " + price_min + " " + price_max + " " + count);
                        }
                    }
                    Generate_product_universal gpu = new Generate_product_universal(brand_txt, weight_txt, txts_for_mix.ToArray(), price_min, price_max, little_type.Name, big_type.Name, u, count, price_rejection, pers_have, amount);
                    gpu.GO();

                }
            }
        }
        public static void Generate_shops()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "gen" + "\\" + "shops";
            string address_txt = path + "\\" + "address.txt";
            string city_txt = path + "\\" + "city.txt";
            string[] address = File.ReadAllLines(address_txt);
            string[] city = File.ReadAllLines(city_txt);
            int shops_count = 0;
            int id = SQL_Commands.HowMuchRows("shops", "ID_shop") + 1;
            if (address.Length < city.Length) shops_count = address.Length; else shops_count = city.Length;
            for (int i = 0; i < shops_count; i++)
            {
                string[] kn = {"shop_address", "shop_city" };
                string[] kv = {address[i], city[i] };
                int key_id = SQL_Commands.TableHaveKey("shops", kn, kv);

                if (key_id == 0)
                {
                    string[] kn_n = { "ID_shop", "shop_address", "shop_city" };
                    string[] kv_n = { id.ToString(), address[i], city[i] };
                    SQL_Commands.WriteInTable("shops", kn_n, kv_n);
                    id++;
                }

            }
        }

        public static void Generate_people(bool auto_email, int count)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "gen" + "\\" + "people";
            string first_male_txt = path + "\\" + "first_male.txt";
            string first_female_txt = path + "\\" + "first_female.txt";
            string second_txt = path + "\\" + "second.txt";
            string third_txt = path + "\\" + "third.txt";
            string[] first_male = File.ReadAllLines(first_male_txt);
            string[] first_female = File.ReadAllLines(first_female_txt);
            string[] second = File.ReadAllLines(second_txt);
            string[] third = File.ReadAllLines(third_txt);
            string f_name = "";
            string s_name = "";
            string t_name = "";
            int id = SQL_Commands.HowMuchRows("customers", "ID_customer") + 1;
            Random r = new Random();
            for (int i = 0; i< count; i++)
            {
                if (r.Next(0,2)>0)
                {
                    f_name = first_female[r.Next(0, first_female.Length)];
                    s_name = second[r.Next(0, second.Length)]+"a";
                    t_name = third[r.Next(0, third.Length)]+"a";
                }
                else
                {
                    f_name = first_male[r.Next(0, first_female.Length)];
                    s_name = second[r.Next(0, second.Length)];
                    t_name = third[r.Next(0, third.Length)];
                }
                string email = "";

                if (auto_email)
                    email = f_name + s_name + t_name + id + "@yandex.ru";
                else
                    email = "diplom2020@yandex.ru";
                string[] kn = { "ID_customer", "FIO_customer","email_customer" };
                string[] kv = { id.ToString(), s_name + " " + f_name + " " + t_name, email };
                SQL_Commands.WriteInTable("customers", kn, kv);
                id++;
            }
        }
        public static void Generate_check()
        { }
    }
}
