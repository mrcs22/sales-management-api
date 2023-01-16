
namespace tech_test_payment_api.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string OrderIdentifier { get; set; }
        public DateTime Date { get; set; }
        public Seller Seller { get; set; }
        public EnumStatusSale Status {get; set;}
        public Product[] Products { get; set; }
    }
}