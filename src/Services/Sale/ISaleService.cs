using tech_test_payment_api.Models;

namespace tech_test_payment_api.Services
{
    public interface ISaleService
    {
        public void CreateSale(Sale sale);
        public Sale GetSaleByIdentifier(string identifier);
        public void ApproveSalePayment(string identifier);
        public void SendSaleToCarrier(string identifier);
        public void FinishSaleDelivery(string identifier);
        public void CancelSale(string identifier);

    }
}