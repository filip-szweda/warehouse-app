using Bogus;
using entity_framework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework
{
    internal class WarehouseInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Items.Any())
            {
                var itemFaker = new Faker<Item>()
                    .RuleFor(i => i.Name, f => f.Commerce.ProductName())
                    .RuleFor(i => i.Price, f => f.Random.Double(1, 1000))
                    .RuleFor(i => i.Stock, f => f.Random.Int(0, 1000));

                var items = itemFaker.Generate(100);
                context.Items.AddRange(items);
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                var clientFaker = new Faker<Client>()
                    .RuleFor(c => c.Name, f => f.Person.FullName)
                    .RuleFor(c => c.Address, f => f.Address.FullAddress());

                var eClientFaker = new Faker<EClient>()
                    .RuleFor(c => c.Name, f => f.Person.FullName)
                    .RuleFor(c => c.Address, f => f.Address.FullAddress())
                    .RuleFor(c => c.IPAddress, f => f.Internet.Ip());

                var clients = clientFaker.Generate(50);
                var eClients = eClientFaker.Generate(50);

                context.Clients.AddRange(clients);
                context.Clients.AddRange(eClients);
                context.SaveChanges();
            }

            /*if (!context.Orders.Any())
            {
                var orderFaker = new Faker<Order>()
                    .RuleFor(o => o.Client, f => f.PickRandom(context.Clients.ToList()))
                    .RuleFor(o => o.Completed, f => f.Random.Bool());

                var eClients = context.Clients.ToList().OfType<EClient>().ToList();
                var eOrderFaker = new Faker<EOrder>()
                    .RuleFor(o => o.Client, f => f.PickRandom(eClients))
                    .RuleFor(o => o.Completed, f => f.Random.Bool())
                    .RuleFor(o => o.IPAddress, f => f.Internet.Ip());

                var orders = orderFaker.Generate(100);
                var eOrders = eOrderFaker.Generate(100);

                context.Orders.AddRange(orders);
                context.Orders.AddRange(eOrders);
                context.SaveChanges();
            }*/
        }
    }
}
