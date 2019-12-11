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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public void AddCategory(Category category)
        {
            this.Categories.Add(category);
            this.SaveChanges();
        }

        public void EditCategory(Category UpdatedCategory)
        {
            Category category = this.Categories.Find(UpdatedCategory.CategoryID);
            category.CategoryName = UpdatedCategory.CategoryName;
            category.Description = UpdatedCategory.Description;
            this.SaveChanges();
        }

        public void EditProduct(Product UpdatedProduct)
        {
            Product product = this.Products.Find(UpdatedProduct.ProductId);
            product.ProductName = UpdatedProduct.ProductName;
            product.UnitPrice = UpdatedProduct.UnitPrice;
            product.Discontinued= UpdatedProduct.Discontinued;
            this.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            this.Products.Remove(product);
            this.SaveChanges();
        }
        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }

    }
}

