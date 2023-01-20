using tech_test_payment_api.Models;

namespace tests.Factories
{
    public class SaleFactory
    {
        public static Sale CreateValidSale(EnumStatusSale status){
            DateTime timestamp = DateTime.Now;
            Seller seller = SellerFactory.CreateValidSeller();

            Sale sale = new Sale {
                Id = 1,
                OrderIdentifier = Guid.NewGuid().ToString() ,
                Date = timestamp,
                Seller = seller,
                Status = status,
                Products= new List<Product>{
                     new Product { Id = 1,
                        Name="Product test",
                        Amount= 42,
                        CreatedAt= timestamp,
                        Price=73
                        }
                    }               
            };

            return sale;
        }
    }
}