using System.ComponentModel.DataAnnotations;

namespace tech_test_payment_api.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [StringLength(11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "cpf must be numeric")]
        public string Cpf { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
       [Required]
       [MinLength(10)]
       [MaxLength(11)]
       [RegularExpression("^[0-9]*$", ErrorMessage = "phoneNumber must be numeric")]
        public string PhoneNumber { get; set; }
    }
}