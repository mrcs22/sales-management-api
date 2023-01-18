using tech_test_payment_api.Models;
using tech_test_payment_api.Repositories;
using tech_test_payment_api.Services.Exceptions;

namespace tech_test_payment_api.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        public void ApproveSalePayment(int id)
        {
            var sale = _saleRepository.GetSaleById(id);

            if(sale.Status != EnumStatusSale.Waiting_payment)
                throw new SaleServiceException($"Sale of id {id} is not waiting for payment");
            
            sale.Status = EnumStatusSale.Payment_accepted;

            _saleRepository.UpdateSale(sale);
        }

        public void CancelSale(int id)
        {
            var sale = _saleRepository.GetSaleById(id);

            bool isSaleWaitingForPaymentOrPaid = (sale.Status == EnumStatusSale.Waiting_payment || sale.Status == EnumStatusSale.Payment_accepted);
            if(!isSaleWaitingForPaymentOrPaid)
                throw new SaleServiceException($"Only sales with status {EnumStatusSale.Waiting_payment} or {EnumStatusSale.Payment_accepted} are cancellable");
            
            sale.Status = EnumStatusSale.Canceled;
            _saleRepository.UpdateSale(sale);
        }

        public void CreateSale(Sale sale)
        {
            throw new NotImplementedException();
        }

        public void FinishSaleDelivery(int id)
        {
            throw new NotImplementedException();
        }

        public Sale GetSaleById(int id)
        {
            throw new NotImplementedException();
        }

        public void SendSaleToCarrier(int id)
        {
            throw new NotImplementedException();
        }
    }
}