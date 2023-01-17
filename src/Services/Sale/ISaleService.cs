using tech_test_payment_api.Models;

namespace tech_test_payment_api.Services
{
    public interface ISaleService
    {
        public void CreateSale(Sale sale);
        public Sale GetSaleById(int id);
        public void ApproveSalePayment(int id);
        public void SendSaleToCarrier(int id);
        public void FinishSaleDelivery(int id);
        public void CancelSale(int id);

    }
}