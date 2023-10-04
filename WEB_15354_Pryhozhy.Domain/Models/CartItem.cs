using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153504_Pryhozhy.Domain.Entities;

namespace WEB_153504_Pryhozhy.Domain.Models
{
    public class CartItem
    {
        public Pizza Pizza { get; set; }
        public int Quantity { get; set; }

        public CartItem(Pizza pizza, int quantity)
        {
            Pizza = pizza;
            Quantity = quantity;
        }
    }
}
