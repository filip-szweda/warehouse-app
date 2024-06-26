﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    public class Order
    {
        [Key]
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
            string text = $"Order [Id]: {Id}, [Completed]: {Completed}, [IsEOrder]: {IsEOrder()}, [Client]: {Client}";
            foreach (var item in OrderItems)
            {
                text += "\n\t\t" + item.ToString();
            }
            text += $"\n\t\t[AmountOfItems]: {AmountOfItems()}, [Total]: {TotalPrice().ToString("0.00")}";
            return text;
        }

        public bool IsEOrder()
        {
            return this is EOrder;
        }
    }
}
