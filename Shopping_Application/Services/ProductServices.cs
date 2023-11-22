using Shopping_Application.IServices;
using Shopping_Application.Models;

namespace Shopping_Application.Services
{
    public class ProductServices : IProductServices
    {
        ShopDbContext context;
        public ProductServices()
        {
            context = new ShopDbContext();
        }
        public bool CreateProduct(Product p)
        {
            try
            {
                context.Products.Add(p);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProduct(Guid id)
        {
            try
            {
                var product = context.Products.Find(id);
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Product> GetAllProducts()
        {
            return context.Products.ToList();
        }
        public Product GetProductById(Guid id)
        {
            return context.Products.FirstOrDefault(p => p.Id == id);
            // return context.Products.SingleOrDefault(p => p.Id == id);
        }
        public List<Product> GetProductsByName(string name)
        {
            return context.Products.Where(p => p.Name.Contains(name)).ToList();
            // select * from Products where name like '%name%'
        }
        public bool UpdateProduct(Product p)
        {
            try
            {
                var product = context.Products.Find(p.Id);
                product.Name = p.Name;
                product.Description = p.Description;
                product.Price = p.Price;
                product.Supplier = p.Supplier;
                product.AvailableQuantity = p.AvailableQuantity;
                product.Status = p.Status;
                context.Products.Update(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
