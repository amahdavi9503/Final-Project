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
                    Console.WriteLine();
                    Console.WriteLine("========= Products ==========");
                    Console.WriteLine("1) Display Products");
                    Console.WriteLine("2) Add Product");
                    Console.WriteLine("3) Edit Product");
                    Console.WriteLine("4) Display a Specific Product");
                    Console.WriteLine();
                    Console.WriteLine("======== Categories =========");
                    Console.WriteLine("5) Display Categories");
                    Console.WriteLine("6) Add Category");
                    Console.WriteLine("7) Edit Category");
                    Console.WriteLine("8) Display a Specific Category");
                    Console.WriteLine();
                    Console.WriteLine("Q to quit");
                    //Console.WriteLine("6) Edit Post");
                    //Console.WriteLine("Enter q to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Option {choice} selected", choice);

                    if (choice == "0")      // Display ALL Product names and IDs
                    {
                        var db = new ProductCategoriesContext();
                        var query = db.Products.OrderBy(b => b.ProductName);

                        foreach (var item in query)
                        {
                            Console.WriteLine($"  {item.ProductId}) {item.ProductName}");
                        }

                        Console.WriteLine($"{query.Count()} Products returned");
                    }

                    else if (choice == "1")      // Display Products
                    {
                        var db = new ProductCategoriesContext();
                        var query = db.Products.OrderBy(b => b.ProductName);
                        var DiscontinuedQuery = db.Products.Where(b => b.Discontinued == true).OrderBy(b => b.ProductName);
                        var ActiveQuery = db.Products.Where(b => b.Discontinued == false).OrderBy(b => b.ProductName);

                        Console.WriteLine("Enter your selection:");
                        Console.WriteLine("1) Display ALL products");
                        Console.WriteLine("2) Display Discontinued Products");
                        Console.WriteLine("3) Display Active Products");
                        string Option = Console.ReadLine();

                        Console.WriteLine("{0,-15}{1,-40}{2,-15}", "ProductID", "Product Name", "Product Status");
                        Console.WriteLine("{0,-15}{1,-40}{2,-15}", "---------", "-------------", "--------------");

                        if (Option == "1")  //Display all products
                        {
                            string ProductStatus;
                            foreach (var item in query)
                            {
                                if (item.Discontinued == false)
                                {
                                    ProductStatus = "Active";
                                }
                                else
                                {
                                    ProductStatus = "Discontinued";
                                }

                                Console.WriteLine($"{item.ProductId,-15}{item.ProductName,-40}{ProductStatus,-15}");
                            }

                            Console.WriteLine();
                            Console.WriteLine($"{query.Count()} Products returned");
                            Console.WriteLine();
                        }

                        else if (Option == "2")  //Display Discontinued Products
                        {
                            foreach (var item in DiscontinuedQuery)
                            {
                                Console.WriteLine($"{item.ProductId,-15}{item.ProductName,-40}{"Discontinued",-15}");
                            }

                            Console.WriteLine();
                            Console.WriteLine($"{DiscontinuedQuery.Count()} Products returned");
                            Console.WriteLine();
                        }

                        else if (Option == "3")  //Display Active Products
                        {
                            foreach (var item in ActiveQuery)
                            {

                                Console.WriteLine($"{item.ProductId,-15}{item.ProductName,-40}{"Active",-15}");
                            }

                            Console.WriteLine();
                            Console.WriteLine($"{ActiveQuery.Count()} Products returned");
                            Console.WriteLine();
                        }

                    }

                    else if (choice == "2")     // Add Product
                    {
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

                                Console.Write("Enter Product's Unit Price: ");
                                product.UnitPrice = Convert.ToDecimal(Console.ReadLine());

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

                    else if (choice == "3")   // Edit Product
                    {
                        // Display Products
                        var db = new ProductCategoriesContext();
                        var query = db.Products.OrderBy(b => b.ProductName);

                        Console.WriteLine($"{query.Count()} Products returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"  {item.ProductId}) {item.ProductName}");
                        }

                        Console.WriteLine();
                        Console.Write("Choose the productID to edit:");
                        Console.WriteLine();

                        int Id = Int32.Parse(Console.ReadLine());
                        Product EditProd = db.Products.FirstOrDefault(p => p.ProductId == Id);

                        if (EditProd != null)
                        {
                            Console.Write($"Enter new Product Name for { EditProd.ProductName}: ");
                            EditProd.ProductName = Console.ReadLine();
                            Console.WriteLine();

                            Console.Write($"Enter new Unit Price: ");
                            EditProd.UnitPrice = Decimal.Parse(Console.ReadLine());
                            Console.WriteLine();

                            db.EditProduct(EditProd);
                            logger.Info("Product (id: {productid}) updated", EditProd.ProductId);
                        }
                    }

                    else if (choice == "4")      // Display a specific product
                    {
                        var db = new ProductCategoriesContext();
                        var query = db.Products.OrderBy(b => b.ProductId);

                        Console.Write("Enter ProductID for the product you want to display: ");
                        Console.WriteLine();

                        if (int.TryParse(Console.ReadLine(), out int ProductId))
                        {
                            if (db.Products.Count(b => b.ProductId == ProductId) == 0)
                            {
                                logger.Error("There are no Products saved with that Id");
                            }
                            else
                            {
                                query = db.Products.Where(p => p.ProductId == ProductId).OrderBy(p => p.ProductName);

                                foreach (var item in query)
                                {
                                    Console.WriteLine($"Product Name: {item.ProductName}\nCategoryID: {item.CategoryId}\n" +
                                                      $"Qty Per Unit: {item.QuantityPerUnit}\n" +
                                                      $"Unit Price: {item.UnitPrice}\nUnits in Stock: {item.UnitsInStock}\n" +
                                                      $"Units on Order: {item.UnitsOnOrder}\nReorder Level: {item.ReorderLevel}\n" +
                                                      $"Discontinued Flag: {item.Discontinued}");   //$"SupplierID: {item.SupplierId}\n
                                }
                            }
                        }
                        Console.WriteLine();
                    }

                    else if (choice == "5")       //Display Categories
                    { 
                        var db = new ProductCategoriesContext();
                        var query = db.Categories.OrderBy(b => b.CategoryName);
 
                        Console.WriteLine("{0,-15}{1,-20}{2,-50}", "Category ID", "Category Name", "Description");
                        Console.WriteLine("{0,-15}{1,-20}{2,-50}", "-----------", "-------------", "------------");

                        foreach (var item in query)
                          {
                               Console.WriteLine($"{item.CategoryID,-15}{item.CategoryName,-20}{item.Description,-50}");
                          }

                        Console.WriteLine();
                        Console.WriteLine($"{query.Count()} Categories returned");
                        Console.WriteLine();                       
                    } 

                    else if (choice == "6")       //Add Category
                    {
                        Console.Write("Enter a name for a new Category: ");
                        var category = new Category { CategoryName = Console.ReadLine() };

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new ProductCategoriesContext();

                            // check for unique name
                            if (db.Categories.Any(b => b.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Category name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");

                                // Enter the rest of the fields for the new category 

                                Console.Write("Enter Category Description: ");
                                category.Description = Console.ReadLine();

                                db.AddCategory(category);
                                logger.Info("Category added - {CategoryName}", category.CategoryName);
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

                    else if (choice == "7")       //Edit Category
                    {
                        //Edit Category
                    }

                    else if (choice == "8")       //Display a Specific Category
                    {
                        //Display a Specific Category
                    }


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
