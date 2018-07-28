namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BabyStore.Models;
    using System.Collections.Generic;
    internal sealed class Configuration : DbMigrationsConfiguration<BabyStore.DAL.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BabyStore.DAL.StoreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            List<Category> categories = new List<Category>
            {
                new Category { Name = "Clothes"},
                new Category { Name = "Toy"},
                new Category { Name = "Feeding"},
                new Category { Name = "Medicine"},
                new Category { Name = "Travel"},
                new Category { Name = "Sleeping"}
            };
            //Perhaps it means: take the category with name cat.Name and update it to "category" if it exists or create category if not
            categories.ForEach(category => context.Categories.AddOrUpdate(cat => cat.Name, category));     
            
            /*To say two Categores having same name, you can do it individually, like this:
             * context.Categories.Add(new Category{ Name = "Clothes"});
             * context.SaveChanges();
             * context.Categories.Add(new Category{ Name = "Clothes"}) ;
             * context.SaveChanges();
             */
            context.SaveChanges();

            List<Product> products = new List<Product>
            {
                new Product {
                    Name = "Sleep Suit",
                    Description = "For sleeping or general waer",
                    Price = 8.44M,
                    CategoryID = categories.Single( c => c.Name == "Clothes").ID
                },
                new Product {
                    Name = "Orange and Yellow Lion",
                    Description = "Makes a squaking noise",
                    Price = 1.99M,
                    CategoryID = categories.Single(c => c.Name == "Toy").ID
                },
                new Product {
                    Name = "Blue Rabbit",
                    Description = "Baby comforter",
                    Price = 2.99M,
                    CategoryID = categories.Single( c => c.Name == "Toy").ID
                },
                new Product {
                    Name = "3 Pack of Bottles",
                    Description = "For a leak free drink everytime",
                    Price = 24.99M,
                    CategoryID = categories.Single( c => c.Name == "Feeding").ID
                },
                new Product {
                    Name = "3 Pack of Bibs",
                    Description = "Keep your baby dry when feeding",
                    Price = 8.99M,
                    CategoryID = categories.Single( c => c.Name == "Feeding").ID
                },
                new Product {
                    Name = "Powered Baby Milk",
                    Description = "Nutritional and Tasty",
                    Price = 9.99M,
                    CategoryID = categories.Single( c => c.Name == "Feeding").ID
                },
                new Product {
                    Name = "Pack of 70 Disposable Nappies",
                    Description = "Dry and secure nappies with snug fit",
                    Price = 19.99M,
                    CategoryID = categories.Single( c => c.Name == "Feeding").ID
                },
                new Product {
                    Name = "Colic Medicine",
                    Description = "For heling with baby colic pains",
                    Price = 4.99M,
                    CategoryID = categories.Single( c => c.Name == "Medicine").ID
                },
                new Product {
                    Name = "Black Pram and Pushchair System",
                    Description = "Convert from pram to pushchair, with raincover",
                    Price = 299.99M,
                    CategoryID = categories.Single( c => c.Name == "Travel").ID
                },
                new Product {
                    Name = "Car Seat",
                    Description = "For safe car travel",
                    Price = 49.99M,
                    CategoryID = categories.Single( c => c.Name == "Travel").ID,
                },
                new Product {
                    Name = "Moses Basket",
                    Description = "Platic moses basket",
                    Price = 75.99M,
                    CategoryID = categories.Single( c => c.Name == "Sleeping").ID
                },
                new Product {
                    Name = "Crib",
                    Description = "Wooden Crib",
                    Price = 35.99M,
                    CategoryID = categories.Single( c => c.Name == "Sleeping").ID
                },
                new Product {
                    Name = "Cot Bed",
                    Description = "Convert from cot into bed for older children",
                    Price = 149.99M,
                    CategoryID = categories.Single( c => c.Name == "Sleeping").ID
                },
                new Product {
                    Name = "Circus Crib Bale",
                    Description = "Contains sheet, duvet",
                    Price = 29.99M,
                    CategoryID = categories.Single( c => c.Name == "Sleeping").ID
                },
                new Product {
                    Name = "Loved Crib Bale",
                    Description = "Contains sheet, duvet and dumper",
                    Price = 35.99M,
                    CategoryID = categories.Single( c => c.Name == "Sleeping").ID
                }

            };
            products.ForEach(product => context.Products.AddOrUpdate(p => p.Name, product));
            context.SaveChanges();
        }
    }
}
