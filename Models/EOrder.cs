using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    public class EOrder : Order
    {
        public string IPAddress { get; set; }

        public override string ToString()
        {
            string text = $"Order [Id]: {Id}, [Completed]: {Completed}, [IsEOrder]: {IsEOrder()}, [Client]: {Client}, [IPAddress]: {IPAddress}";
            foreach (var item in OrderItems)
            {
                text += "\n\t\t" + item.ToString();
            }
            text += $"\n\t\t[AmountOfItems]: {AmountOfItems()}, [Total]: {TotalPrice().ToString("0.00")}";
            return text;
        }
    }
}
