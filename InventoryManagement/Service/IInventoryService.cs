using InventoryManagement.Model;

namespace InventoryManagement.Service
{
    public interface IInventoryService
    {
        /// <summary>
        /// Gets all products from source system
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts();

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product? GetProduct(string id);

        /// <summary>
        /// Searches all products from the source system, using the callback provided.
        /// </summary>
        /// <returns></returns>
        public List<Product> SearchProducts(string searchString, Func<string, Product, bool> searchCallback);

        /// <summary>
        /// Adds a product to the source system.
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product);

        /// <summary>
        /// Updates a product in the source system.
        /// </summary>
        /// <param name="product"></param>
        public bool UpdateProduct(Product product);
    }
}
