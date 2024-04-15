using System.Drawing;

namespace InventoryManagement.Model
{
    public class Product
    {
        public Product(string id, dynamic size, string name, string color, string category)
        {
            Id = id;
            Size = size;
            Name = name;
            Color = color;
            Category = category;
        }

        public string Id { get; }
        public dynamic? Size { get; set; } 
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Category { get; set; }

     
    }
}