using tech_test_payment_api.Models;

namespace tech_test_payment_api.Services
{
    public interface ISellerService
    {
        public bool ValidateSeller(Seller seller);
    }
}