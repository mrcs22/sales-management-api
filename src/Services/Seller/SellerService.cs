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
            Seller sellerFound = _sellerRepository.GetSellerByCpfOrEmail(seller.Cpf, seller.Email);
            if(sellerFound == null)
                return true;

            if(seller.Name != sellerFound.Name)
                return false;

            if(seller.Cpf != sellerFound.Cpf)
                return false;
            
            if(seller.Email != sellerFound.Email)
                return false;
            
            if(seller.PhoneNumber != sellerFound.PhoneNumber)
                return false;
          
            return true;
        }  
    }
}