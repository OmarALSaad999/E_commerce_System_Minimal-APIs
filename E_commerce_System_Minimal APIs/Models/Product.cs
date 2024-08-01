using System.ComponentModel.DataAnnotations;

namespace E_commerce_System_Minimal_APIs.Models
{
    public class Product
    {

        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        // Navigation Property. It is used in order to access the related Category Object
        public Category? Category { get; set; }
    }
}
