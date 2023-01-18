using tech_test_payment_api.Models;

namespace tests.Factories
{
    public class SaleFactory
    {
        public static Sale CreateValidSale(EnumStatusSale status){
            DateTime timestamp = DateTime.Now;
            
            Sale sale = new Sale {
                Id = 1,
                OrderIdentifier = Guid.NewGuid().ToString() ,
                Date = timestamp,
                Seller = new Seller{
                    Id = 0,
                    Name = "Seller Test",
                    Cpf="01234567890",
                    Email="seller-test@test.com",
                    PhoneNumber="99999999999"
                },
                Status = status,
                Products= new List<Product>{
                     new Product { Id = 1,
                        Name="Product test",
                        Amount= 42,
                        CreatedAt= timestamp
                        }
                    }               
            };

            return sale;
        }
    }
}