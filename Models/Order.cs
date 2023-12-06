using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Client Client { get; set; }

        public int ClientId { get; set; }
        public bool Completed { get; set; }

        public double TotalPrice()
        {
            return OrderItems.Sum(item => item.Item.Price * item.Quantity);
        }

        public int AmountOfItems()
        {
            return OrderItems.Sum(item => item.Quantity);
        }

        public override string ToString()
        {
            string text = $"Order Id {Id}";
            foreach (var item in OrderItems)
            {
                text += "\t" + item.ToString() + "\n";
            }
            return text;
        }
    }
}
