using tech_test_payment_api.Models;
using tech_test_payment_api.Data;

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

        public Sale GetSaleById(int id)
        {
            var sale = _context.Sales.Find(id);
            return sale;
        }

        public void UpdateSale(Sale sale)
        {
            _context.Update(sale);
            _context.SaveChanges();
        }
    }
}