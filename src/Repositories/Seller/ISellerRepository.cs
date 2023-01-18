using tech_test_payment_api.Models;

namespace tech_test_payment_api.Repositories
{
    public interface ISellerRepository
    {
        public Seller GetSellerByCpf(string cpf);
    }
}