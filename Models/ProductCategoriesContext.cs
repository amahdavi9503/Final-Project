using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class ProductCategoriesContext : DbContext
    {
         public ProductCategoriesContext() : base("name=ProductContext") { }
        public DbSet<Category> Cateogries { get; set; }
        public DbSet<Product> Products { get; set; }

        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }

    }
}

