using InventoryManagement.Model;
using InventoryManagement.Source;

namespace InventoryManagement.Service
{
    internal class InventoryService : IInventoryService
    {
        protected readonly IInventorySource _inventorySource;

        public InventoryService(IInventorySource inventorySource)
        {
            _inventorySource = inventorySource;
        }

        /// <summary>
        /// Gets all products from source system
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts()
        {
            return _inventorySource.GetAll();
        }

        /// <summary>
        /// Add a product to the source system.
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            _inventorySource.Add(product);
        }

        /// <summary>
        /// Searches all products from the source system, using the callback provided.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="searchCallback"></param>
        /// <returns></returns>
        public List<Product> SearchProducts(string searchString, Func<string, Product, bool> searchCallback)
        {
            var allProducts = _inventorySource.GetAll();
            return allProducts.FindAll(p => searchCallback(searchString, p)).ToList();
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product? GetProduct(string id)
        {
            return _inventorySource.Get(id);
        }

        /// <summary>
        /// Updates a product in the source system.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool UpdateProduct(Product product)
        {
            return _inventorySource.Update(product);
        }
    }
}
