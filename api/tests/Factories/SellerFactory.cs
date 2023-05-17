using tech_test_payment_api.Models;
namespace tests.Factories
{
    public class SellerFactory
    {
        public static Seller CreateValidSeller(
            string name = "Seller Test",
            string cpf="01234567890",
            string email="seller-test@test.com",
            string phoneNumber="99999999999"
            ){
            
            Seller seller = new Seller{
                Id = 1,
                Name = name ,
                Cpf=cpf,
                Email=email,
                PhoneNumber=phoneNumber
            };
            
            return seller;
        }
    }
}