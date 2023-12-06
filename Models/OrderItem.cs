using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    internal class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"[Item]: {Item.Name}, [Quantity]: {Quantity}";
        }
    }
}
