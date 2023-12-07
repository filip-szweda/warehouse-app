using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity_framework.Models
{
    public class ItemWarehouse
    {
        private readonly AppDbContext _dbContext;

        public ItemWarehouse(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddNewItem(Item item)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var existingItem = _dbContext.Items.FirstOrDefault(i => i.Name == item.Name);

                if (existingItem == null)
                {
                    _dbContext.Items.Add(item);
                }
                else
                {
                    existingItem.Stock += item.Stock;
                }

                _dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        public void IncreaseItemStock(string itemName, int stock)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var existingItem = _dbContext.Items.FirstOrDefault(i => i.Name == itemName);

                if (existingItem == null)
                {
                    return;
                }
                else
                {
                    existingItem.Stock += stock;
                }

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
