using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_product_generation
{



    /// <summary>
    /// Класс для генерации продуктов на любых основаниях
    /// </summary>
    class Generate_product_universal
    {
        private List<string> mix = new List<string>();
        private string[] lines_brand;
        private List<string[]> mix_txts = new List<string[]>();
        private string[] lines_weight;
        private string little_type;
        private string big_type;
        private int price_min;
        private int price_max;
        private int count;
        private UNIT unit;
        private int pers_have;
        private int price_rejection;
        private int amount;
        //private MySqlConnection connection;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="txt_brand"></param> имя файла со списком брэндов
        /// <param name="txt_weight"></param> имя файла, с возможным весом/объемом продукта
        /// <param name="txts_for_mix"></param> список имен файлов с прочими параментрами для генерации
        /// <param name="cost_min"></param> - минимальная цена за 100г/100мл/1шт
        /// <param name="cost_max"></param> - максимальная цена за 100г/100мл/1шт 
        /// <param name="little_type"></param> -меньший тип (кошачья еда влажная)
        /// <param name="big_type"></param> - больший тип (товары ждля животных) 
        /// <param name="unit"></param> - Единицы измерения
        public Generate_product_universal(string txt_brand, string txt_weight, string[] txts_for_mix, int price_min, int price_max, string little_type, string big_type, UNIT unit, int count, int price_rejection, int pers_have, int amount)
        {
            this.lines_brand = File.ReadAllLines(txt_brand);
            this.lines_weight = File.ReadAllLines(txt_weight);
            this.little_type = little_type;
            this.big_type = big_type;
            this.price_min = price_min;
            this.price_max = price_max;
            this.unit = unit;
            this.count = count;
            this.price_rejection = price_rejection;
            this.pers_have = pers_have;
            this.amount = amount;
            foreach (string fn in txts_for_mix)
            {
                this.mix_txts.Add(File.ReadAllLines(fn));
            }
        }

        /// <summary>
        /// Создает строки,все варианты
        /// </summary>
        private void Mix()
        {
            Random r = new Random();
            foreach (string brand in lines_brand)
            {
                double cost = cost_generation(r);
               // Console.WriteLine("Cost = " + cost);
                foreach (string s_weight in lines_weight)
                {
                    double weight = Convert.ToDouble(s_weight);
                    double full_cost = 0;
                    full_cost = weight * cost;
                    string s_norm_weight = unit_normolizer(weight);
                    List<string> mix_types = new List<string>();
                    for (int i = 0; i < mix_txts.Count - 1; i++)
                    {
                        mix_types = GoMix(mix_txts[i], mix_txts[i + 1]);
                    }
                    foreach (string mt in mix_types)
                    {
                        mix.Add(brand + "_" + mt + "_" + s_norm_weight + " " + full_cost);
                    }
                }

            }

        }

        /// <summary>
        /// Миксует два списка общих описаний продукта
        /// </summary>
        /// <param name="list1"></param> - список в наличии
        /// <param name="list2"></param> - список, с которым надо смешать
        /// <returns></returns>
        private List<string> GoMix(string[] list1, string[] list2)
        {
            List<string> mix = new List<string>();
            foreach (string l1 in list1)
            {
                foreach (string l2 in list2)
                {
                    mix.Add(l1 + "_" + l2);
                }
            }
            return mix;
        }

        /// <summary>
        /// Печатает полученую строку, для отладки
        /// </summary>
        private void print_mix()
        {
            foreach (string s in mix)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("--------------------------------------------------------------------");
        }

        /// <summary>
        /// Отладка
        /// </summary>
        public void GO()
        {
            Mix();
            delete_part(count);
           // print_mix();
            write_brand();
            write_big_type();
            write_small_type();
            write_product();
          //  print_mix();
            write_stores();
        }

        /// <summary>
        /// Удаление рандомных строк для уменяьшения кол-ва записей
        /// </summary>
        /// <param name="count_produst"></param> сколько записей должно остаться (максимум)
        private void delete_part(int count_produst)
        {
            Random rnd = new Random();
            int count_for_delete = mix.Count - count_produst;
            for (int i = 0; i < count_for_delete; i++)
            {
                int r = rnd.Next(0, mix.Count - 1);
                mix.RemoveAt(r);
            }


        }

        /// <summary>
        /// Генерирует цену для бренда на единицу измерения (1 грамм, 1 мллитр, 1 штука)
        /// </summary>
        /// <returns></returns>
        private double cost_generation(Random rnd)
        {
            double price = rnd.Next(price_min, price_max + 1);
            if (unit == UNIT.GRAM || unit == UNIT.MILILITRE)
            {
                price = price / 100;
            }
            return price;
        }

        /// <summary>
        /// Нормализует единицы измерения (1000г = 1кг, 1000мл = 1л)
        /// </summary>
        /// <returns></returns>
        private string unit_normolizer(double weight)
        {
            string un = "";
            switch (unit)
            {
                case UNIT.PIECE:
                    un = weight + "шт";
                    break;
                case UNIT.GRAM:
                    if (weight >= 1000)
                    {
                        un = weight / 1000 + "кг";
                    }
                    else
                    {
                        un = weight + "г";
                    }
                    break;
                case UNIT.MILILITRE:
                    if (weight >= 1000)
                    {
                        un = weight / 1000 + "л";
                    }
                    else
                    {
                        un = weight + "мл";
                    }
                    break;
            }

            return un;
        }
        /// <summary>
        /// Запись брэндов
        /// </summary>
        private void write_brand()
        {
          //  DataTable table = SQL_Commands.TryToConnect_Full("brands");

            int id = 1;
            string[] kn = { "brand_name" };

            foreach (string brand in lines_brand)
            {
                string[] kv = { brand };
                int key_id = SQL_Commands.TableHaveKey("brands", kn, kv);
                id = SQL_Commands.HowMuchRows("brands", "ID_brand") + 1;
                //if (key_id == 0) Console.WriteLine("нет"); else Console.WriteLine("есть");
                if (key_id == 0)
                {
                    string[] kn_w = { "ID_brand", "brand_name" };
                    string[] kv_w = { id.ToString(), brand };
                    SQL_Commands.WriteInTable("brands", kn_w, kv_w);
                    id++;
                }
            }
        }
        /// <summary>
        /// Запись больших типов (товары для животных)
        /// </summary>
        private void write_big_type()
        {
           // DataTable table = SQL_Commands.TryToConnect_Full("product_type_big");

            int id = 1;
            string[] kn = { "name_product_type_big" };
            string[] kv = { big_type };
            int key_id = SQL_Commands.TableHaveKey("product_type_big", kn, kv);
            id = SQL_Commands.HowMuchRows("product_type_big", "ID_product_type_big") + 1;
            //if (key_id == 0) Console.WriteLine("нет"); else Console.WriteLine("есть");
            if (key_id == 0)
            {
                string[] kn_w = { "ID_product_type_big", "name_product_type_big" };
                string[] kv_w = { id.ToString(), big_type };
                SQL_Commands.WriteInTable("product_type_big", kn_w, kv_w);
                id++;
            }
        }

        /// <summary>
        /// Запись малых типов (кошачья еда)
        /// </summary>
        private void write_small_type()
        {
           // DataTable table = SQL_Commands.TryToConnect_Full("product_type_little");

            int id = 1;
            string[] kn_b = { "name_product_type_big" };
            string[] kv_b = { big_type };
            int key_id_b = SQL_Commands.TableHaveKey("product_type_big", kn_b, kv_b);

            string[] kn = { "name_product_type_little" };
            string[] kv = { little_type };
            int key_id = SQL_Commands.TableHaveKey("product_type_little", kn, kv);
            id = SQL_Commands.HowMuchRows("product_type_little", "ID_product_type_little") + 1;
            //if (key_id == 0) Console.WriteLine("нет"); else Console.WriteLine("есть");
            if (key_id == 0)
            {
                string[] kn_w = { "ID_product_type_little", "ID_product_type_bigger", "name_product_type_little" };
                string[] kv_w = { id.ToString(), key_id_b.ToString(), little_type };
                SQL_Commands.WriteInTable("product_type_little", kn_w, kv_w);
                id++;
            }
        }
        /// <summary>
        /// запись продуктов
        /// </summary>
        private void write_product()
        {
            //Felix_для-иммунитета_Утка_150г 93
            int id = SQL_Commands.HowMuchRows("products", "ID_product") + 1;
            //int i = 0;
            for (int i = 0; i<mix.Count; i++ )
            {
                string[] kn = { "product_name", "type_little_name", "brand_name" };
                string[] kv = { mix[i].Split(' ')[0], little_type, mix[i].Split('_')[0] };
                int key_id = SQL_Commands.TableHaveKey("products", kn, kv);

                if (key_id == 0)
                {
                    string cost = mix[i].Split(' ')[1].Replace(',', '.');
                    string[] kn_w = { "ID_product", "product_name", "type_little_name", "brand_name", "product_cost" };
                    string[] kv_w = { id.ToString(), mix[i].Split(' ')[0], little_type, mix[i].Split('_')[0], mix[i].Split(' ')[1].Replace(',', '.') };
                    SQL_Commands.WriteInTable("products", kn_w, kv_w);
                    mix[i] += " " + id;
                    id++;
                }
                else mix[i] = "false";
            }
         
        }
        /// <summary>
        /// Заполнениес кладов
        /// </summary>
        private void write_stores()
        {//Felix_для-иммунитета_Утка_150г 93 1
            //3 Гагарина 33 Челябинск
            DataTable table_shop = SQL_Commands.TryToConnect_Full("shops");
            //int id = SQL_Commands.HowMuchRows("products", "ID_product_store");
            Random r = new Random();
            foreach (string p in mix)
            {
                if (!p.Equals("false"))
                {
                    foreach (DataRow s in table_shop.Rows)
                    {
                       // object[] shop_string = s.ItemArray;
                        int go = r.Next(1, 101);
                        //Console.WriteLine("выпало "+ go + " id товара" + p.Split(' ')[2] + " ид магазина "+ shop_string[0].ToString()) ;
                        if (go <= pers_have)
                        {
                            object[] shop_string = s.ItemArray;
                            //Console.WriteLine("ПРОШЛО выпало " + go + " id товара" + p.Split(' ')[2] + " ид магазина " + shop_string[0].ToString());
                            string[] kn_p = { "ID_product_store", "ID_shop_store" };
                            string[] kv_p = { p.Split(' ')[2], shop_string[0].ToString() };
                            int key_id = SQL_Commands.TableHaveKey("product_on_store", kn_p, kv_p);
                            if (key_id == 0)
                            {
                                int am_in_shop = r.Next(0, amount + 1);
                                double price = Convert.ToDouble(p.Split(' ')[1].Replace('.', ','));
                                price += price * (r.Next(-price_rejection, price_rejection + 1))/100;
                                string[] kn = { "ID_product_store", "ID_shop_store", "product_amount", "product_price" };
                                string[] kv = { p.Split(' ')[2], shop_string[0].ToString(), am_in_shop.ToString(), price.ToString().Replace(',', '.') };
                                SQL_Commands.WriteInTable("product_on_store", kn, kv);
                            }
                        }
                    }
                }
            }
        }

    }
}
