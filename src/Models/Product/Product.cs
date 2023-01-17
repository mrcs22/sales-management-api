namespace tech_test_payment_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }

        public DateTime CreatedAt {get; set;}
    }
}