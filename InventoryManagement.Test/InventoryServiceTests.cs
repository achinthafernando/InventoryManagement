using InventoryManagement.Model;
using InventoryManagement.Service;
using InventoryManagement.Source;
using NSubstitute;
using NUnit.Framework;

namespace InventoryManagement.Test
{
    [TestFixture]
    internal class InventoryServiceTests
    {
        /// <summary>
        /// Test to verify that GetAllProducts returns all products
        /// </summary>
        [Test]
        public void GetAllProducts_WhenCalled_ReturnsAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
               new Product("100", "20", "New Product", "Category", "Color"),
               new Product("200", "30", "New Product 2", "Category 2", "Red"),
               new Product("300", "40", "New Product 3", "Category 3", "Red")

            };
            var mockInventorySource = Substitute.For<IInventorySource>();
            mockInventorySource.GetAll().Returns(expectedProducts);

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            var actualProducts = inventoryService.GetAllProducts();

            // Assert
            Assert.That(expectedProducts.Count, Is.EqualTo(actualProducts.Count));
        }

        /// <summary>
        /// Test to verify that AddProduct adds a product
        /// </summary>
        [Test]
        public void AddProduct_Adds_Product()
        {
            // Arrange
            var productToAdd = new Product("300", "40", "New Product 3", "Category 3", "Red");

            var mockInventorySource = Substitute.For<IInventorySource>();

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            inventoryService.AddProduct(productToAdd);

            // Assert
            mockInventorySource.Received().Add(productToAdd);
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
            var mockInventorySource = Substitute.For<IInventorySource>();
            mockInventorySource.GetAll().Returns(expectedProducts);

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            var actualProducts = inventoryService.SearchProducts(searchString, (searchString, product) => product.Name.Contains(searchString));

            // Assert
            Assert.That(expectedProducts.Count, Is.EqualTo(actualProducts.Count));
        }

        /// <summary>
        /// Test to verify that GetProduct returns product by ID
        /// </summary>
        [Test]
        public void GetProduct_WhenCalled_ReturnsProduct()
        {
            // Arrange
            var productId = "100";
            var expectedProduct = new Product("100", "20", "New Product", "Category", "Color");

            var mockInventorySource = Substitute.For<IInventorySource>();
            mockInventorySource.Get(productId).Returns(expectedProduct);

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            var actualProduct = inventoryService.GetProduct(productId);

            // Assert
            Assert.That(expectedProduct, Is.EqualTo(actualProduct));
        }

        /// <summary>
        /// Test case to verify that UpdateProduct returns true if product is updated
        /// </summary>
        [Test]  
        public void UpdateProduct_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var productToUpdate = new Product("100", "20", "New Product", "Category", "Color");

            var mockInventorySource = Substitute.For<IInventorySource>();
            mockInventorySource.Update(productToUpdate).Returns(true);

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            var result = inventoryService.UpdateProduct(productToUpdate);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Test to verify that UpdateProduct returns false if product not found
        /// </summary>
        [Test]
        public void UpdateProduct_WhenCalled_ReturnsFalse()
        {
            // Arrange
            var productToUpdate = new Product("100", "20", "New Product", "Category", "Color");

            var mockInventorySource = Substitute.For<IInventorySource>();
            mockInventorySource.Update(productToUpdate).Returns(false);

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            var result = inventoryService.UpdateProduct(productToUpdate);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Test to verify that UpdateProduct returns false if product not found
        /// </summary>
        [Test] 
        public void UpdateProduct_WhenProductNotFound_ReturnsFalse()
        {
            // Arrange
            var productToUpdate = new Product("999", "20", "New Product", "Category", "Color");

            var mockInventorySource = Substitute.For<IInventorySource>();
            mockInventorySource.Update(productToUpdate).Returns(false);

            var inventoryService = new InventoryService(mockInventorySource);

            // Act
            var result = inventoryService.UpdateProduct(productToUpdate);

            // Assert
            Assert.That(result, Is.False);
        }   
    }
}
