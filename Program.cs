using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace FinalProject
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("Enter your selection:");
                    Console.WriteLine("1) Display all products");
                    Console.WriteLine("2) Add Product***");
                    Console.WriteLine("3) Edit Product");
                    Console.WriteLine("4) Display All Products");
                    Console.WriteLine("5) Display a Specific Product");
                    //Console.WriteLine("6) Edit Post");
                    //Console.WriteLine("Enter q to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Option {choice} selected", choice);

                    if (choice == "1")
                    {
                        // Display Products
                        var db = new ProductCategoriesContext();
                        var query = db.Products.OrderBy(b => b.ProductName);

                        Console.WriteLine($"{query.Count()} Products returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.ProductName);
                        }
                    }
                    else if (choice == "2")
                    {
                        // Add Product
                        Console.Write("Enter a name for a new Product: ");
                        var product = new Product { ProductName = Console.ReadLine() };                

                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if (isValid)
                        {
                            var db = new ProductCategoriesContext();
                            // check for unique name
                            if (db.Products.Any(b => b.ProductName == product.ProductName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Product name exists", new string[] { "ProductName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");

                               // Enter the rest of the fields for the new product 
                                Console.Write("Enter CategoryID for the new Product: ");
                                product.CategoryId = Convert.ToInt32(Console.ReadLine());

                                Console.Write("Enter Product Status: A = Active; D = Discontinued: ");
                                string selection = Console.ReadLine().ToUpper();
                                if (selection == "A")
                                {
                                    product.Discontinued = false;
                                }
                                else if (selection == "D")
                                {
                                    product.Discontinued = true;
                                }
                                else
                                {
                                    logger.Error("Ivalid option. Enter A or D");
                                }

                                // Save Product to db
                                db.AddProduct(product);
                                logger.Info("Product added - {ProductName}", product.ProductName);
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    //  else if (choice == "3")
                    //  {
                    //      // Create Post
                    //      var db = new BloggingContext();
                    //      var query = db.Blogs.OrderBy(b => b.BlogId);

                    //      Console.WriteLine("Select the blog you would to post to:");
                    //      foreach (var item in query)
                    //      {
                    //          Console.WriteLine($"{item.BlogId}) {item.Name}");
                    //      }
                    //      if (int.TryParse(Console.ReadLine(), out int BlogId))
                    //      {
                    //          if (db.Blogs.Any(b => b.BlogId == BlogId))
                    //          {
                    //              Post post = InputPost(db);
                    //              if (post != null)
                    //              {
                    //                  post.BlogId = BlogId;
                    //                  db.AddPost(post);
                    //                  logger.Info("Post added - {title}", post.Title);
                    //              }
                    //          }
                    //          else
                    //          {
                    //              logger.Error("There are no Blogs saved with that Id");
                    //          }
                    //      }
                    //      else
                    //      {
                    //          logger.Error("Invalid Blog Id");
                    //      }
                    //  }
                    //  else if (choice == "4")
                    //  {
                    //      // Display Posts
                    //      var db = new BloggingContext();
                    //      var query = db.Blogs.OrderBy(b => b.BlogId);
                    //      Console.WriteLine("Select the blog's posts to display:");
                    //      Console.WriteLine("0) Posts from all blogs");
                    //      foreach (var item in query)
                    //      {
                    //          Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");
                    //      }

                    //      if (int.TryParse(Console.ReadLine(), out int BlogId))
                    //      {
                    //          IEnumerable<Post> Posts;
                    //          if (BlogId != 0 && db.Blogs.Count(b => b.BlogId == BlogId) == 0)
                    //          {
                    //              logger.Error("There are no Blogs saved with that Id");
                    //          }
                    //          else
                    //          {
                    //              // display posts from all blogs
                    //              Posts = db.Posts.OrderBy(p => p.Title);
                    //              if (BlogId == 0)
                    //              {
                    //                  // display all posts from all blogs
                    //                  Posts = db.Posts.OrderBy(p => p.Title);
                    //              }
                    //              else
                    //              {
                    //                  // display post from selected blog
                    //                  Posts = db.Posts.Where(p => p.BlogId == BlogId).OrderBy(p => p.Title);
                    //              }
                    //              Console.WriteLine($"{Posts.Count()} post(s) returned");
                    //              foreach (var item in Posts)
                    //              {
                    //                  Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                    //              }
                    //          }
                    //      }
                    //      else
                    //      {
                    //          logger.Error("Invalid Blog Id");
                    //      }
                    //  }
                    //  else if (choice == "5")
                    //  {
                    //      // delete post
                    //      Console.WriteLine("Choose the post to delete:");
                    //      var db = new BloggingContext();
                    //      var post = GetPost(db);
                    //      if (post != null)
                    //      {
                    //          db.DeletePost(post);
                    //          logger.Info("Post (id: {postid}) deleted", post.PostId);
                    //      }
                    //  }
                    else if (choice == "3")
                    {
                        // Edit Product
                        Console.WriteLine("Choose the productID to edit:");
                        var db = new ProductCategoriesContext();
                        var product = GetProduct(db);
                        if (product != null)
                        {
                            // Input Product
                            Product UpdatedProduct = InputProduct(db);
                            if (UpdatedProduct != null)
                            {                             
                                UpdatedProduct.ProductId = product.ProductId;

                                Console.WriteLine($"Enter new Product Name for : { product.ProductName}");
                                UpdatedProduct.ProductName = product.ProductName;

                                Console.WriteLine($"Enter new Unit Price for : { product.ProductName}");
                                UpdatedProduct.UnitPrice = product.UnitPrice;

                                Console.WriteLine($"Active or Discontinued? (A/D) : { product.ProductName}");
                                UpdatedProduct.Discontinued = product.Discontinued;

                                db.EditProduct(UpdatedProduct);
                                logger.Info("Product (id: {productid}) updated", UpdatedProduct.ProductId);
                            }
                        }
                    }  

                        Console.WriteLine();
                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }

        public static Product InputProduct(ProductCategoriesContext db)
        {
            Product product = new Product();
            Console.WriteLine("Enter the Product Name");
            product.ProductName = Console.ReadLine();
            Console.WriteLine("Enter the Product Category ID");
            product.CategoryId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Product Status: A = Active; D = Discontinued: ");
                if (Console.ReadLine().ToUpper() == "A")
                {
                    product.Discontinued = false;
                }
                else if (Console.ReadLine().ToUpper() == "D")
                {
                    product.Discontinued = true;
                }
                else
                {
                    logger.Error("Ivalid option. Enter A or D");
                }



            ValidationContext context = new ValidationContext(product, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(product, context, results, true);
            if (isValid)
            {
                return product;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }

        public static Product GetProduct(ProductCategoriesContext db)
        {
            // Display all Categories and Products
            // Force Eager Loading of Products
            var categories = db.Categories.Include("Products").OrderBy(b => b.CategoryName);
            foreach (Category b in categories)
            {
                Console.WriteLine(b.CategoryName);
                if (b.Products.Count() == 0)
                {
                    Console.WriteLine($"  <No Products>");
                }
                else
                {
                    foreach (Product p in b.Products)
                    {
                        Console.WriteLine($"  {p.ProductId}) {p.ProductName}");
                    }
                }
            }
            if (int.TryParse(Console.ReadLine(), out int ProductId))
            {
                Product product = db.Products.FirstOrDefault(p => p.ProductId == ProductId);
                if (product != null)
                {
                    return product;
                }
            } 

            logger.Error("Invalid Product Id");
            return null;
        }  

    } 
}
