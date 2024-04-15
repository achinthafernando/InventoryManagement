using InventoryManagement.Model;

namespace InventoryManagement.Source
{
    public interface IInventorySource
    {
        public List<Product> GetAll();
        public Product? Get(string id);
        public void Add(Product product);
        public bool Update(Product product);
    }
}