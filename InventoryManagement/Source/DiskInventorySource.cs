using InventoryManagement.Model;
using System.Text.Json;

namespace InventoryManagement.Source
{
    /// <summary>
    /// Disk storage implementation of IInventoryStorage.
    /// Instantiated with a JSON file name and performs CRUD operations to that file.
    /// </summary>
    internal class DiskInventorySource : IInventorySource
    {
        private readonly string _filePath;
        public DiskInventorySource(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Get a product by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Product? Get(string id)
        {
            var products = GetAll();
            return products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Get all products from the source system.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return products ?? new List<Product>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add a product to the source system.
        /// </summary>
        /// <param name="product"></param>
        public void Add(Product product)
        {
            try
            {
                var products = GetAll();
                products.Add(product);
                WriteToJsonFile(products);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update a product in the source system.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Update(Product product)
        {
            try
            {
                var products = GetAll();
                var index = products.FindIndex(p => p.Id == product.Id);
                if (index != -1)
                {
                    products[index] = product;
                    WriteToJsonFile(products);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Write to the JSON file.
        /// </summary>
        /// <param name="products"></param>
        private void WriteToJsonFile(List<Product> products)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var jsonString = JsonSerializer.Serialize(products, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
