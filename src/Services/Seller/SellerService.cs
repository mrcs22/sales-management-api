using tech_test_payment_api.Models;
using tech_test_payment_api.Repositories;

namespace tech_test_payment_api.Services
{
    
    public class SellerService : ISellerService
    {   private readonly ISellerRepository _sellerRepository;

        public SellerService(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }
        public bool ValidateSeller(Seller seller)
        {
            var sellerPropertiesToValidate = new List<string>{ "Name", "Cpf", "Email", "PhoneNumber"};
            bool isSellerValid = true;
            
            var sellerFound = _sellerRepository.GetSellerByCpfOrEmail(seller.Cpf, seller.Email);
            sellerPropertiesToValidate.ForEach(prop => {
                bool isPropValid = seller.GetType().GetProperty(prop) == sellerFound.GetType().GetProperty(prop);
                
                if(!isPropValid)
                    isSellerValid = false;
            });

            return isSellerValid;
        }
    }
}