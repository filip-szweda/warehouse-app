using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public double AmountSpent()
        {
            return Orders.Where(o => o.Completed).Sum(o => o.TotalPrice());
        }

        public override string ToString()
        {
            return $"{Name} - [Address]: {Address}";
        }

        public bool IsEClient()
        {
            return this is EClient;
        }
    }
}
