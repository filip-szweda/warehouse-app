using entity_framework.Models;

namespace entity_framework
{
    internal class Program
    {
        private static AppDbContext _dbContext;
        private static ItemWarehouse _itemWarehouse;
        private static void Main(string[] args)
        {
            _dbContext = new AppDbContext();
            _itemWarehouse = new ItemWarehouse(_dbContext);
            WarehouseInitializer.Initialize(_dbContext);

            var quit = false;
            while (!quit)
            {
                Console.WriteLine("<<< warehouse_client >>>");
                Console.WriteLine("0. Show Items");
                Console.WriteLine("1. Add New Item");
                Console.WriteLine("2. Increase Item Stock");
                Console.WriteLine("3. Quit");

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
                    default:
                        Console.WriteLine("[ERROR] Invalid option, choose option 0, 1, 2 or 3.");
                        break;
                }
            }
        }

        private static void ShowItemsInWarehouse()
        {
            var items = _dbContext.Items.ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        private static void AddNewItemToWarehouse()
        {
            Console.WriteLine("Enter Item name: ");
            var itemName = Console.ReadLine();

            Console.WriteLine("Enter Item price: ");
            var itemPrice = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Item stock: ");
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
            Console.WriteLine("Enter Item name: ");
            var itemName = Console.ReadLine();

            Console.WriteLine("Enter Item stock: ");
            var itemStock = int.Parse(Console.ReadLine());

            _itemWarehouse.IncreaseItemStock(itemName, itemStock);
        }
    }
}