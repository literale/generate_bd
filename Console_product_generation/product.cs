using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_product_generation
{
    class Product
    {
        private int produc_ID = 0;
        private int product_amount = 0;
        private double product_price = 0;
        //public Product(int product_id, int product_amount, double product_price)
        //{
        //    this.produc_ID = product_id;
        //    this.product_price = product_price;
        //    this.product_amount = product_amount;
        //}
         public static Product Return_random_product(int ID_shop)
        {
            Product pr = new Product();

            return pr;
        }
    }
}
