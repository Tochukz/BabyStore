using BabyStore.Models;
using System.Data.Entity;

namespace BabyStore.DAL
{
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { set; get; }
        public DbSet<Category> Categories { set; get; }
    }
}