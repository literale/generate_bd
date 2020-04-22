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
        private int cost_min;
        private int cost_max;
        private int percent_cost_vague;
        private UNIT unit;
        private MySqlConnection connection;

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
        public Generate_product_universal(string txt_brand, string txt_weight, string[] txts_for_mix, int cost_min, int cost_max, string little_type, string big_type, UNIT unit, int percent_cost_vague)
        {
            this.lines_brand = File.ReadAllLines(txt_brand);
            this.lines_weight = File.ReadAllLines(txt_weight);
            this.little_type = little_type;
            this.big_type = big_type;
            this.cost_min = cost_min;
            this.cost_max = cost_max;
            this.unit = unit;
            this.percent_cost_vague = percent_cost_vague;
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
            foreach (string brand in lines_brand)
            {
                double cost = cost_generation();
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
        public void GO(int max_rows)
        {
            Mix();
            delete_part(max_rows);
            print_mix();
            write_brand();
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
        private double cost_generation()
        {
            Random rnd = new Random();
            double cost = rnd.Next(cost_min, cost_max + 1);
            if (unit == UNIT.GRAM || unit == UNIT.MILILITRE)
            {
                cost = cost / 100;
            }
            return cost;
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
            DataTable table = SQL_Commands.TryToConnect_Full("Brands");

            int id = 1;
            bool brand_exist = false;
            string[] kn = { "Brand_name" };

            foreach (string brand in lines_brand)
            {
                string[] kv = {brand};
                brand_exist = SQL_Commands.TableHaveKey("Brands", kn, kv);
                Console.WriteLine(brand_exist.ToString());
                if (!brand_exist)
                {
                    id = SQL_Commands.HowMuchRows("Brands", "idBrands");
                    string[] kn_w = { "idBrands", "Brand_name" };
                    string[] kv_w = { id.ToString(), brand };
                    SQL_Commands.WriteInTable("Brands", kn_w, kv_w);
                }
            }
        }
        /// <summary>
        /// Запись больших типов (товары для животных)
        /// </summary>
        private void write_big_type()
        { }
        /// <summary>
        /// Запись малых типов (кошачья еда)
        /// </summary>
        private void write_small_type()
        {
        
        }
        /// <summary>
        /// запись продуктов
        /// </summary>
        private void write_product()
        {

        }
        /// <summary>
        /// Заполнениес кладов
        /// </summary>
        private void write_stores()
        {

        }

    }
}
