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

            var quit = false;
            while (!quit)
            {
                Console.WriteLine("<<< warehouse_client >>>");
                Console.WriteLine("0. Add New Item");
                Console.WriteLine("1. Increase Item Stock");
                Console.WriteLine("3. Quit");

                var option = Console.ReadLine();
                switch (option)
                {
                    case "0":
                        AddNewItemToWarehouse();
                        break;
                    case "1":
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