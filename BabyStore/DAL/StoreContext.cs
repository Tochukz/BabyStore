using BabyStore.Models;
using System.Data.Entity;

namespace BabyStore.DAL
{
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductImageMapping> ProductImageMappings { set; get; }
    }
}