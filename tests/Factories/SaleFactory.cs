using tech_test_payment_api.Models;

namespace tests.Factories
{
    public class SaleFactory
    {
        public static Sale CreateValidSale(EnumStatusSale status){
            DateTime timestamp = DateTime.Now;
            Seller seller = SellerFactory.CreateValidSeller();
            Product product = ProductFactory.CreateValidProduct();

            Sale sale = new Sale {
                Id = 1,
                OrderIdentifier = Guid.NewGuid().ToString() ,
                Date = timestamp,
                Seller = seller,
                Status = status,
                Products= new List<Product>{product}               
            };

            return sale;
        }

        public static Sale CreateValidSaleBody(){
            Seller seller = SellerFactory.CreateValidSeller();
            Product product = ProductFactory.CreateValidProduct();

            Sale validSaleBody = new Sale {
                Seller = new Seller {
                        Name = seller.Name,
                        Cpf= seller.Cpf,
                        Email= seller.Email,
                        PhoneNumber= seller.PhoneNumber
                    },                     
                Products = new List<Product> {
                    new Product {      
                        Name = product.Name,
                        Amount = product.Amount,
                        Price = product.Price    
                    }
                }
            };

            return validSaleBody;
          
        }
    }
}