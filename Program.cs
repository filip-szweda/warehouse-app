﻿using entity_framework.Models;

namespace entity_framework
{
    internal class Program
    {
        private static AppDbContext _dbContext;
        private static ItemWarehouse _itemWarehouse;
        private static Client _activeClient;
        private static void Main(string[] args)
        {
            _dbContext = new AppDbContext();
            _itemWarehouse = new ItemWarehouse(_dbContext);
            WarehouseInitializer.Initialize(_dbContext);

            var quit = false;
            while (!quit)
            {
                Console.Clear();

                Console.WriteLine("<<< warehouse_client >>>");
                Console.WriteLine("0. Show Items");
                Console.WriteLine("1. Add New Item");
                Console.WriteLine("2. Increase Item Stock");
                Console.WriteLine("3. Quit");
                Console.WriteLine("4. Choose Active Client");
                Console.WriteLine("5. Add New Order");

                var option = Console.ReadLine();
                switch (option)
                {
                    case "0":
                        ShowItemsInWarehouse();
                        break;
                    case "1":
                        AddNewItemToWarehouse();
                        break;
                    case "2":
                        IncreaseItemStockInWarehouse();
                        break;
                    case "3":
                        quit = true;
                        break;
                    case "4":
                        _activeClient = ChooseActiveClient();
                        break;
                    case "5":
                        AddNewOrder();
                        break;
                    default:
                        Console.WriteLine("[ERROR] Invalid option, choose option 0, 1, 2 or 3.");
                        break;
                }
            }
        }

        private static void ShowItemsInWarehouse()
        {
            Console.Clear();

            var items = _dbContext.Items.ToList();
            foreach (var item in items)
            {
                Console.WriteLine($"\t{item}");
            }

            Console.ReadLine();
        }

        private static void AddNewItemToWarehouse()
        {
            Console.Clear();

            Console.WriteLine("[INFO] Enter Item name: ");
            var itemName = Console.ReadLine();

            Console.WriteLine("[INFO] Enter Item price: ");
            var itemPrice = double.Parse(Console.ReadLine());

            Console.WriteLine("[INFO] Enter Item stock: ");
            var itemStock = int.Parse(Console.ReadLine());

            var item = new Item
            {
                Name = itemName,
                Price = itemPrice,
                Stock = itemStock
            };

            _itemWarehouse.AddNewItem(item);
        }

        private static void IncreaseItemStockInWarehouse()
        {
            Console.Clear();

            Console.WriteLine("[INFO] Enter Item name: ");
            var itemName = Console.ReadLine();

            Console.WriteLine("[INFO] Enter Item stock: ");
            var itemStock = int.Parse(Console.ReadLine());

            _itemWarehouse.IncreaseItemStock(itemName, itemStock);
        }

        private static Client ChooseActiveClient()
        {
            string phrase = "";
            while (true)
            {
                Console.Clear();

                var clients = _dbContext.Clients.ToList();
                if (phrase != "")
                {
                    clients = clients.Where(c => c.Name.Contains(phrase)).ToList();
                }
                
                foreach (var client in clients)
                {
                    Console.WriteLine($"\t{client}");
                }
                Console.WriteLine("[INFO] Enter Client index to choose Active Client or enter phrase to filter");

                var input = Console.ReadLine();
                if (int.TryParse(input, out var index))
                {
                    if (index < 0 || index >= clients.Count)
                    {
                        Console.WriteLine("[ERROR] Invalid index, choose index between 0 and {0}", clients.Count - 1);
                        continue;
                    }
                    return clients[index];
                }
                else
                {
                    phrase = input;
                }
            }
        }

        private static void AddNewOrder()
        {
            var order = new Order
            {
                Client = _activeClient,
                Completed = false
            };

            while (true)
            {
                Console.Clear();

                Console.WriteLine("[INFO] Enter Item name: ");
                var itemName = Console.ReadLine();

                var item = _dbContext.Items.FirstOrDefault(i => i.Name == itemName);
                if (item == null)
                {
                    Console.WriteLine("[ERROR] Item with name {0} does not exist", itemName);
                    return;
                }

                Console.WriteLine("[INFO] Enter Item quantity: ");
                var itemQuantity = int.Parse(Console.ReadLine());

                var orderItem = new OrderItem
                {
                    Item = item,
                    Quantity = itemQuantity
                };
                order.OrderItems.Add(orderItem);
                _dbContext.SaveChanges();

                var input = "";
                while(input != "y" && input != "n")
                {
                    Console.WriteLine("[INFO] Finish Order? (y/n)");
                    input = Console.ReadLine();
                }
                
                if (input == "n")
                {
                    return;
                }
            }
        }
    }
}