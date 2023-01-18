using tech_test_payment_api.Data;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Repositories 
{
    public class SellerRepository : ISellerRepository
    {
        private readonly AppDbContext _context;

        public SellerRepository(AppDbContext context)
        {
            _context = context;
        }
        public Seller GetSellerByCpfOrEmail(string cpf, string email)
        {
            var seller = _context.Sellers.FirstOrDefault(s => s.Cpf == cpf || s.Email == email);

            return seller;
        }
    }
}