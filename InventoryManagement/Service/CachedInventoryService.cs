using InventoryManagement.Model;

namespace InventoryManagement.Service
{
    internal class CachedInventoryService: IInventoryService
    {
        private readonly IInventoryService _inventoryService;
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(5);
        private List<Product> _cache;
        private DateTime _lastCacheUpdateTime;

        public CachedInventoryService(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            RefreshCache();
        }

        /// <summary>
        /// Gets all products from source system
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts()
        {
            try
            {
                if (IsCacheExpired())
                {
                    RefreshCache();
                }
                return _cache;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds a product to the source system.
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            try
            {
                _inventoryService.AddProduct(product);
                RefreshCache();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Searches all products from the source system, using the callback provided.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="searchCallback"></param>
        /// <returns></returns>
        public List<Product> SearchProducts(string searchString, Func<string, Product, bool> searchCallback)
        {
            try
            {
                if (IsCacheExpired())
                {
                    RefreshCache();
                }
                // Perform search on cached products
                return _cache.FindAll(p => searchCallback(searchString, p));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product? GetProduct(string id)
        {
            try
            {
                if (IsCacheExpired())
                {
                    RefreshCache();
                }
                // Find product in cached products
                return _cache.Find(p => p.Id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a product in the source system.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool UpdateProduct(Product product)
        {
            try
            {
                var updated = _inventoryService.UpdateProduct(product);
                if (updated)
                {
                    RefreshCache();
                }
                return updated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if the cache is expired.
        /// </summary>
        /// <returns></returns>
        private bool IsCacheExpired()
        {
            return DateTime.Now - _lastCacheUpdateTime > _cacheTimeout;
        }

        /// <summary>
        /// Refreshes the cache.
        /// </summary>
        private void RefreshCache()
        {
            try
            {
                _cache = _inventoryService.GetAllProducts();
                _lastCacheUpdateTime = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
