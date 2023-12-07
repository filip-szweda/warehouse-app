using entity_framework.Models;
using Microsoft.EntityFrameworkCore;

namespace entity_framework
{
    internal class Program
    {
        private static AppDbContext _dbContext;
        private static ItemWarehouse _itemWarehouse;
        private static Client _activeClient = null;
        private static void Main(string[] args)
        {
            _dbContext = new AppDbContext();
            _itemWarehouse = new ItemWarehouse(_dbContext);
            WarehouseInitializer.Initialize(_dbContext);

            var quit = false;
            while (!quit)
            {
                Console.Clear();

                if(_activeClient != null)
                {
                    Console.WriteLine($"<<< Active Client: {_activeClient} >>>");
                }
                else
                {
                    Console.WriteLine("<<< No Active Client >>>");
                }

                Console.WriteLine("<<< Warehouse >>>");
                Console.WriteLine("0. Show Items");
                Console.WriteLine("1. Add New Item");
                Console.WriteLine("2. Increase Item Stock");
                Console.WriteLine("3. Choose Active Client");
                Console.WriteLine("4. Add New Order");
                Console.WriteLine("5. Show Orders");
                Console.WriteLine("6. Show Clients who ordered selected Item");
                Console.WriteLine("7. Quit");

                var option = Console.ReadLine();
                switch (option)
                {
                    case "0":
                        ShowItems();
                        break;
                    case "1":
                        AddNewItem();
                        break;
                    case "2":
                        IncreaseItemStock();
                        break;
                    case "3":
                        _activeClient = ChooseActiveClient();
                        break;
                    case "4":
                        AddNewOrder();
                        break;
                    case "5":
                        ShowOrders();
                        break;
                    case "6":
                        ShowClientWhoOrderedSelectedItem();
                        break;
                    case "7":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("[ERROR] Invalid option, choose option from 0 to 7.");
                        break;
                }
            }
        }

        private static void ShowItems()
        {
            Console.Clear();

            var items = _dbContext.Items.ToList();
            foreach (var item in items)
            {
                Console.WriteLine($"\t{item}");
            }

            Console.WriteLine("[INFO] Press Enter to Continue");
            Console.ReadLine();
        }

        private static void AddNewItem()
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

        private static void IncreaseItemStock()
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
                        Console.WriteLine("[INFO] Press Enter to Continue");
                        Console.ReadLine();
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
            if (_activeClient == null)
            {
                Console.Clear();
                Console.WriteLine($"[ERROR] No Active Client");
                Console.WriteLine("[INFO] Press Enter to Continue");
                Console.ReadLine();
                return;
            }

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
                    Console.WriteLine("[INFO] Press Enter to Continue");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("[INFO] Enter Item quantity: ");
                var itemQuantity = int.Parse(Console.ReadLine());

                var orderItem = new OrderItem
                {
                    Order = order,
                    Item = item,
                    Quantity = itemQuantity
                };
                order.OrderItems.Add(orderItem);

                var input = "";
                while(input != "y" && input != "n")
                {
                    Console.WriteLine("[INFO] Finish Order? (y/n)");
                    input = Console.ReadLine();
                }
                
                if (input == "y")
                {
                    _dbContext.Orders.Add(order);
                    _dbContext.SaveChanges();
                    return;
                }
            }
        }

        private static void ShowOrders()
        {
            var currentPage = 0;

            var orders = _dbContext.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToList();
            if (orders.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("[ERROR] There are currently no Orders");
                Console.WriteLine("[INFO] Press Enter to Continue");
                Console.ReadLine();
                return;
            }

            var ordersDict = new Dictionary<int, List<Order>>();
            for (int i = 0; i < orders.Count; i++)
            {
                var key = i / 5;
                if (!ordersDict.ContainsKey(key))
                {
                    ordersDict[key] = new List<Order>();
                }
                ordersDict[key].Add(orders[i]);
            }

            while (true)
            {
                Console.Clear();

                Console.WriteLine($"[INFO] Page no. {currentPage}");
                foreach (var order in ordersDict[currentPage])
                {
                    Console.WriteLine($"\t{order}");
                }

                bool validInput = false;
                while (!validInput)
                {
                    Console.WriteLine("[INFO] Press Enter to Continue / n - next page / p - previous page / <index> - accept Order ");

                    var input = Console.ReadLine();
                    if (int.TryParse(input, out var index))
                    {
                        if (index < 0 || index >= 5)
                        {
                            Console.WriteLine("[ERROR] Invalid index, choose index between 0 and 5");
                            continue;
                        }

                        AcceptOrder(ordersDict[currentPage][index]);
                        validInput = true;
                    }
                    else if (input == "n")
                    {
                        if (currentPage == ordersDict.Count - 1)
                        {
                            Console.WriteLine("[ERROR] You are on the last page");
                            continue;
                        }

                        currentPage++;
                        validInput = true;
                    }
                    else if (input == "p")
                    {
                        if (currentPage == 0)
                        {
                            Console.WriteLine("[ERROR] You are on the first page");
                            continue;
                        }

                        currentPage--;
                        validInput = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private static void ShowClientWhoOrderedSelectedItem()
        {
            Console.Clear();

            Console.WriteLine("[INFO] Enter Item name: ");
            var itemName = Console.ReadLine();

            var item = _dbContext.Items.FirstOrDefault(i => i.Name == itemName);
            if (item == null)
            {
                Console.WriteLine("[ERROR] Item with name {0} does not exist", itemName);
                Console.WriteLine("[INFO] Press Enter to Continue");
                Console.ReadLine();
                return;
            }

            var orders = _dbContext.Orders
                .Where(o => o.OrderItems
                .Any(oi => oi.ItemId == item.Id))
                .Include(o => o.Client)
                .ToList();
            var clients = orders
                .Select(o => o.Client)
                .Distinct()
                .ToList();
            foreach (var client in clients)
            {
                Console.WriteLine($"\t{client}");
            }

            Console.WriteLine("[INFO] Press Enter to Continue");
            Console.ReadLine();
        }

        private static void AcceptOrder(Order order)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                foreach (var orderItem in order.OrderItems)
                {
                    if(orderItem.Item.Stock < orderItem.Quantity)
                    {
                        throw (new Exception($"Not enough stock of {orderItem.Item.Name} to complete order"));
                    }
                    orderItem.Item.Stock -= orderItem.Quantity;
                }

                order.Completed = true;

                _dbContext.Orders.Update(order);
                _dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
    }
}