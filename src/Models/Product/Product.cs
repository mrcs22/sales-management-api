using System.ComponentModel.DataAnnotations;
namespace tech_test_payment_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }
        public DateTime CreatedAt {get; set;}
    }
}