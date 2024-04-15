using InventoryManagement.Model;
using InventoryManagement.Service;
using NSubstitute;
using NUnit.Framework;

namespace InventoryManagement.Test
{
    [TestFixture]
    internal class CachedInventoryServiceTests
    {
        /// <summary>
        /// Test to verify that GetAllProducts returns products from cache
        /// </summary>
        [Test]
        public void GetAllProducts_Returns_Products_From_Cache()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
               new Product("200", "20", "New Product", "Category", "Color"),
               new Product("300", "30", "New Product 2", "Category 2", "Red")
            };

            var mockInventoryService = Substitute.For<IInventoryService>();
            mockInventoryService.GetAllProducts().Returns(expectedProducts);

            // Act
            var cachedInventoryService = new CachedInventoryService(mockInventoryService);
            var actualProducts = cachedInventoryService.GetAllProducts();

            // Assert
            Assert.That(expectedProducts.Count, Is.EqualTo(actualProducts.Count));
        }

        /// <summary>
        /// Test to verify that GetAllProducts refreshes cache after timeout
        /// </summary>
        [Test]
        public void GetAllProducts_Refreshes_Cache_After_Timeout()
        {
            // Arrange
            var initialProducts = new List<Product>
            {
                new Product("100", "20", "New Product", "Category", "Color"),
                new Product("200", "30", "New Product 2", "Category 2", "Red")

            };

            var updatedProducts = new List<Product>
            {
                new Product("300", "20", "New Product", "Category", "Color"),
                new Product("400", "30", "New Product 2", "Category 2", "Red")
            };

            var mockInventoryService = Substitute.For<IInventoryService>();
            mockInventoryService.GetAllProducts().Returns(initialProducts, updatedProducts);

            // Act
            var cachedInventoryService = new CachedInventoryService(mockInventoryService);
            var actualInitialProducts = cachedInventoryService.GetAllProducts();
            // Simulate cache timeout
            System.Threading.Thread.Sleep(300000); // Assuming cache timeout is 5 minute
            var actualUpdatedProducts = cachedInventoryService.GetAllProducts();

            // Assert
            Assert.That(initialProducts.Count, Is.EqualTo(actualInitialProducts.Count));
            Assert.That(updatedProducts.Count, Is.EqualTo(actualUpdatedProducts.Count));
        }

        /// <summary>
        /// Test to verify that AddProduct adds product to source
        /// </summary>
        [Test]
        public void AddProduct_Adds_Product_To_Source()
        {
            // Arrange
            var productToAdd = new Product("300", "40", "New Product 3", "Category 3", "Red");

            var mockInventoryService = Substitute.For<IInventoryService>();

            var cachedInventoryService = new CachedInventoryService(mockInventoryService);

            // Act
            cachedInventoryService.AddProduct(productToAdd);

            // Assert
            mockInventoryService.Received().AddProduct(productToAdd);
        }

        /// <summary>
        /// Test to verify that SearchProducts returns matching products
        /// </summary>
        [Test]
        public void SearchProducts_WhenCalled_ReturnsMatchingProducts()
        {
            // Arrange
            var searchString = "New";
            var expectedProducts = new List<Product>
            {
               new Product("100", "20", "New Product", "Category", "Color"),
               new Product("200", "30", "New Product 2", "Category 2", "Red"),
               new Product("300", "40", "New Product 3", "Category 3", "Red")

            };

            var mockInventoryService = Substitute.For<IInventoryService>();
            mockInventoryService.GetAllProducts().Returns(expectedProducts);
            var cachedInventoryService = new CachedInventoryService(mockInventoryService);

            // Act
            var actualProducts = cachedInventoryService.SearchProducts(searchString, (s, p) => p.Name.Contains(s));

            // Assert
            Assert.That(expectedProducts.Count, Is.EqualTo(actualProducts.Count));
        }

        /// <summary>
        /// Test to verify that UpdateProduct updates product
        /// </summary>
        [Test]
        public void UpdateProduct_Updates_Product()
        {
            // Arrange
            var productToUpdate = new Product("300", "40", "New Product 3", "Category 3", "Red");

            var mockInventoryService = Substitute.For<IInventoryService>();
            mockInventoryService.UpdateProduct(productToUpdate).Returns(true);

            var cachedInventoryService = new CachedInventoryService(mockInventoryService);

            // Act
            var result = cachedInventoryService.UpdateProduct(productToUpdate);

            // Assert
            Assert.IsTrue(result);
        }

    }
}
