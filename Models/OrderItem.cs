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
            return $"Client: {Order.Client.Name}; Item: {Item.Name} ; Completed: {Order.Completed} ;Zam. el. {IsEOrder()} ;NoItems: {Order.AmountOfItems()}, Total: {Order.TotalPrice().ToString("0.00")}";
        }

        private bool IsEOrder()
        {
            return Order is EOrder;
        }
    }
}
