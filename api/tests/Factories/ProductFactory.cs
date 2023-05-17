using tech_test_payment_api.Models;
namespace tests.Factories
{
    public class ProductFactory
    {
            public static Product CreateValidProduct(){
            
                Product product = new Product { 
                    Id = 1,
                    Name="Product test",
                    Amount= 42,
                    CreatedAt= DateTime.Now,
                    Price=73
                };
                
                return product;
        }
    }
}
