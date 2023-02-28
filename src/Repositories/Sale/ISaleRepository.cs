using tech_test_payment_api.Models;

namespace tech_test_payment_api.Repositories
{
    public interface ISaleRepository
    {
        public void CreateSale(Sale sale);
        public Sale GetSaleByIdentifier(string identifier);
        public void UpdateSale(Sale sale);

    }
}