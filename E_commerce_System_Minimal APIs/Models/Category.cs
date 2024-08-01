using System.ComponentModel.DataAnnotations;
namespace E_commerce_System_Minimal_APIs.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
