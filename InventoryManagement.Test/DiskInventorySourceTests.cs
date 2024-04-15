using InventoryManagement.Model;
using InventoryManagement.Source;
using NUnit.Framework;

namespace InventoryManagement.Test
{
    [TestFixture]
    internal class DiskInventorySourceTests
    {
        private const string TestFilePath = "test_products.json";

        [OneTimeSetUp]
        public void SetUp()
        {
            // Create a test JSON file with sample data
            var json = "[{\"id\":\"100\",\"size\":{\"h\":30,\"w\":28},\"name\":\"Slim fit jeans\",\"color\":\"Washed\",\"category\":\"Pants\"},{\"id\":\"200\",\"size\":37,\"name\":\"Summer heels\",\"color\":\"White\",\"category\":\"Shoes\"}]";
            File.WriteAllText(TestFilePath, json);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Delete the test JSON file
            File.Delete(TestFilePath);
        }

        /// <summary>
        /// Test to verify that GetAll returns products from JSON file
        /// </summary>
        [Test]
        public void GetAll_Returns_Products_From_JSON_File()
        {
            // Arrange
            var diskInventorySource = new DiskInventorySource(TestFilePath);

            // Act
            var products = diskInventorySource.GetAll();

            // Assert
            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("100", products[0].Id);
            Assert.AreEqual("Slim fit jeans", products[0].Name);
            Assert.AreEqual("200", products[1].Id);
            Assert.AreEqual("Summer heels", products[1].Name);
        }

        /// <summary>
        /// Test to verify that Get returns product by ID
        /// </summary>
        [Test]
        public void Get_Returns_Product_By_Id()
        {
            // Arrange
            var diskInventorySource = new DiskInventorySource(TestFilePath);

            // Act
            var product = diskInventorySource.Get("100");

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual("100", product!.Id);
            Assert.That(product.Name, Is.EqualTo("Slim fit jeans"));
        }

        /// <summary>
        /// Test to verify that Get returns null if product not found
        /// </summary>
        [Test]
        public void Get_Returns_Null_If_Product_Not_Found()
        {
            // Arrange
            var diskInventorySource = new DiskInventorySource(TestFilePath);

            // Act
            var product = diskInventorySource.Get("999");

            // Assert
            Assert.IsNull(product);
        }

        /// <summary>
        /// Test to verify that Add adds product to file
        /// </summary>
        [Test]
        public void Add_Adds_Product_To_File()
        {
            // Arrange
            var diskInventorySource = new DiskInventorySource(TestFilePath);
            var newProduct = new Product("300", "20", "New Product", "Category", "Color");

            // Act
            diskInventorySource.Add(newProduct);
            var products = diskInventorySource.GetAll();

            // Assert
            Assert.AreEqual(3, products.Count);
            Assert.AreEqual("300", products[2].Id);
            Assert.AreEqual("New Product", products[2].Name);
        }

        /// <summary>
        /// Test to verify that Update updates product in file
        /// </summary>
        [Test]
        public void Update_Updates_Product_In_File()
        {
            // Arrange
            var diskInventorySource = new DiskInventorySource(TestFilePath);
            var updatedProduct = new Product("100", "20", "Updated Product", "Category", "Color");

            // Act
            var success = diskInventorySource.Update(updatedProduct);
            var products = diskInventorySource.GetAll();

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual("Updated Product", products[0].Name);
        }

        /// <summary>
        /// Test to verify that Update returns false if product not found
        /// </summary>
        [Test]
        public void Update_Returns_False_If_Product_Not_Found()
        {
            // Arrange
            var diskInventorySource = new DiskInventorySource(TestFilePath);
            var updatedProduct = new Product("999", "35", "Updated Product", "Category", "Color");

            // Act
            var success = diskInventorySource.Update(updatedProduct);

            // Assert
            Assert.IsFalse(success);
        }
    }
}
