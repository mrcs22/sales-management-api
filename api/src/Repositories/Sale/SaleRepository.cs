using tech_test_payment_api.Models;
using tech_test_payment_api.Data;
using Microsoft.EntityFrameworkCore;

namespace tech_test_payment_api.Repositories
{
        
    public class SaleRepository : ISaleRepository
    {   
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateSale(Sale sale)
        {
            _context.Sales.Add(sale);
            _context.SaveChanges();
        }

        public Sale GetSaleByIdentifier(string identifier)
        {
            var sale = _context.Sales
                .Include(s => s.Seller)
                .Include(s => s.Products)
                .FirstOrDefault(s => s.OrderIdentifier == identifier);
            
            return sale;
        }

        public void UpdateSale(Sale sale)
        {
            _context.Update(sale);
            _context.SaveChanges();
        }
    }
}