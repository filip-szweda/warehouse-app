using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    internal class Item
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public override string ToString()
        {
            return $"{Name} - [Price]: {Price:0.00}$, [Stock]: {Stock}";
        }
    }
}
