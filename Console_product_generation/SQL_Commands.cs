using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_product_generation
{
    class SQL_Commands
    {
        private static MySqlConnection connection;

        public static void setConnection(string connection_string)
        {
            connection = new MySqlConnection(connection_string);
        }
        /// <summary>
        /// закрытие соединения
        /// </summary>
        /// <param name="connection"></param>
        public static DataTable TryToConnect_Full(string table)
        {
            //string connection_string = Info.connection_string;
            connection.Open();
            string request = "SELECT * FROM " + table + ";";
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            //foreach (DataRow s in temp_dtable.Rows)
            //{
            //    object[] fields = s.ItemArray;
            //    foreach (object o in fields)
            //    {
            //        Console.Write(o.ToString() + " ");
            //    }
            //    Console.WriteLine();
            //}
            connection.Close();
            return temp_dtable;
        }

        /// <summary>
        /// запись в таблицу, строка должна иметь значения всех столбцов в формате "('v1', 'v2')"
        /// </summary>
        /// <param name="string_to_write"></param> - строка со значениями для записи
        /// <param name="table"></param> - таблица для записи
        public static void WriteInTable(string table, string[] key_names, string[] key_values)
        {
            //string connection_string = Info.connection_string;
            connection.Open();
            string request = "INSERT INTO " + table + " (";
            for (int i = 0; i < key_names.Length; i++)
            {
                request += key_names[i];
                if (i < key_names.Length - 1) request += ", ";
                else request += ") ";
            }
            request += "VALUES (";
            for (int i = 0; i < key_values.Length; i++)
            {
                request += "'" + key_values[i] + "'";
                if (i < key_values.Length - 1) request += ", ";
                else request += ") ";
            }
            request += ";";
           // Console.WriteLine(request);
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            connection.Close();

        }

        /// <summary>
        /// проверяет, есть ли значение в таблице
        /// </summary>
        /// <param name="table"></param> таблица откуда берем
        /// <param name="key_names"></param> Названия полей
        /// <param name="key_values"></param> Значения полей
        /// request SELECT * FROM brands WHERE idBrands = 01 AND Brand_name = "jjj";
        /// <returns></returns>
        public static int TableHaveKey(string table, string[] key_names, string[] key_values)
        {

            string request = "SELECT * FROM " + table + " WHERE";
            for (int i = 0; i < key_names.Length; i++)
            {
                request += " " + key_names[i] + " = '" + key_values[i] + "'";
                if (i + 1 < key_names.Length) request += " AND ";
            }
            request += ";";
            connection.Open();
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();
            return temp_dtable.Rows.Count;

        }

        /// <summary>
        /// Скоролько уникальных значений в столбце в таблице
        /// </summary>
        /// <param name="table"></param> таблица
        /// <param name="key"></param> столбец
        /// <returns></returns>
        public static int HowMuchRows(string table, string key)
        {
            int id = 0;
            connection.Open();
            string request = "SELECT COUNT(" + key + ") FROM " + table;
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            foreach (DataRow s in temp_dtable.Rows)
            {
                object[] fields = s.ItemArray;
                foreach (object o in fields)
                {
                    connection.Close();
                    return Convert.ToInt32(o);
                }
            }

            connection.Close();
            return id;
        }
    }
}
