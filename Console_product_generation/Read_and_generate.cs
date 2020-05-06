using Org.BouncyCastle.Crypto.Tls;
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
                    Console.WriteLine("Генерация тип " + big_type + " подтип " + little_type);
                    //  Console.WriteLine(little_type.Name);
                    bool empty = true;
                    foreach (DirectoryInfo dir in little_type.GetDirectories())
                    {
                        empty = false;                     
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
                    if (!empty)
                    {
                        Generate_product_universal gpu = new Generate_product_universal(brand_txt, weight_txt, txts_for_mix.ToArray(), price_min, price_max, little_type.Name, big_type.Name, u, count, price_rejection, pers_have, amount);
                        gpu.GO();
                    }

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
            //int d_per_count = count / 10;
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
                    f_name = first_male[r.Next(0, first_male.Length)];
                    s_name = second[r.Next(0, second.Length)];
                    t_name = third[r.Next(0, third.Length)];
                }
                string email = "";

                if (auto_email)
                    email = f_name + s_name + t_name + id + "@yandex.ru";
                else
                    email = "ESGdiplom2020@yandex.ru";
                string[] kn = { "ID_customer", "FIO_customer","email_customer" };
                string[] kv = { id.ToString(), s_name + " " + f_name + " " + t_name, email };
                SQL_Commands.WriteInTable("customers", kn, kv);
                id++;
                //if((i + 1)%d_per_count == 0 && (i+1)>= d_per_count)
                //{
                //    Console.WriteLine((Convert.ToDouble(i+1)/Convert.ToDouble(count))*100+ "%");
                //}
            }
        }
        public static void Generate_check(int count)
        {
            //Вытаскиваем список одного магазина
            //случайно берем из него типы продуктов и продукты
            //ставим их значения
            //и прикручиваем человека
            //Вы вликолепны!
            double avType = 0;
            double avSize = 0;
            int ID_shop = 0;
            int id_check = 0;
            int prod_count = 0;
            double amount = 0;
            Random r = new Random();
            List<int> check_type = new List<int>();
            List<int> check_size = new List<int>();
            id_check = SQL_Commands.HowMuchRows("checks", "ID_check") + 1;
            Console.WriteLine("РАЗМЕРЫ ЧЕКОВ");
            for (int i = 0; i<count; i++)
            {
                ID_shop = r.Next(1, SQL_Commands.HowMuchRows("shops", "ID_shop") + 1);
                Math_help type_mah = new Math_help();
                int type = type_mah.SampleGaussian(r, 8, 10, 1, 20);
                check_type.Add(type);
                Check check = new Check();
                Math_help count_math = new Math_help();
                prod_count = check.check_count(type, r, count_math);
                check_size.Add(prod_count);
                amount = check.Generate_check(ID_shop, id_check, prod_count);
                
                Console.WriteLine("Тип чека№ "+ id_check+" " + check_type[i] + " ; Товаров в чеке: " + check_size[i]);
                avSize += check_size[i];
                avType += check_type[i];
                id_check++;
            }
           
         
            Console.WriteLine("Средний тип чека = " + avType / check_type.Count + " ; Средний размер чека = " + avSize / check_size.Count);


        }

        public static void Random_country()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "gen" + "\\" + "products";
            //Console.WriteLine(path);
            DirectoryInfo di = new DirectoryInfo(path);
            string brand_txt = "";

            foreach (DirectoryInfo big_type in di.GetDirectories())
            {
                // Console.WriteLine(big_type.Name);
                foreach (DirectoryInfo little_type in big_type.GetDirectories())
                {
                    //Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Генерация тип " + big_type + " подтип " + little_type);
                    //  Console.WriteLine(little_type.Name);
                    foreach (DirectoryInfo dir in little_type.GetDirectories())
                    {
                        if (dir.Name.Equals("brand"))
                        {
                            Random r = new Random();
                            FileInfo[] files = dir.GetFiles();
                            brand_txt = path + "\\" + big_type + "\\" + little_type + "\\" + dir.Name + "\\" + files[0].Name;
                            string[] lines_brand = File.ReadAllLines(brand_txt);
                            string count = "F:\\учеба\\Вода(диплом)\\generate_bd\\Console_product_generation\\bin\\Debug\\gen\\count.txt";
                            string[] coutries = File.ReadAllLines(count);
                            List<string> lines_brand2 = new List<string>();
                            foreach (string brand in lines_brand)
                            {
                                if (brand.Contains("~"))
                                {
                                    lines_brand2.Add(brand);
                                }
                                else lines_brand2.Add(brand + "~" + coutries[r.Next(0,coutries.Length)]);
                            }
                            File.WriteAllLines(brand_txt, lines_brand2.ToArray());
                        }

                    }
               }

               
            }
        }
    }
}
