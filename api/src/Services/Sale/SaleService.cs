using tech_test_payment_api.Models;
using tech_test_payment_api.Repositories;
using tech_test_payment_api.Services.Exceptions;

namespace tech_test_payment_api.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISellerRepository _sellerRepository;
        

        public SaleService(ISaleRepository saleRepository, ISellerRepository sellerRepository)
        {
            _saleRepository = saleRepository;
            _sellerRepository = sellerRepository;
        }
        public void CreateSale(Sale sale)
        {
            var timestamp = DateTime.Now;
            
            sale.Date = timestamp;
            sale.OrderIdentifier = Guid.NewGuid().ToString();
            sale.Status = EnumStatusSale.Waiting_payment;

            sale.Products.ForEach(p => p.CreatedAt = timestamp);

            var seller = _sellerRepository.GetSellerByCpfOrEmail(sale.Seller.Cpf, sale.Seller.Email);
            if(seller != null)
                sale.Seller = seller;
            
            _saleRepository.CreateSale(sale);
        }
        public void ApproveSalePayment(string identifier)
        {
            var sale = _saleRepository.GetSaleByIdentifier(identifier);

            if(sale == null)
                throw new SaleNotFoundException("Sale not found");

            if(sale.Status != EnumStatusSale.Waiting_payment)
                throw new SaleServiceException($"Sale of idenfier {identifier} is not waiting for payment");
            
            sale.Status = EnumStatusSale.Payment_accepted;

            _saleRepository.UpdateSale(sale);
        }

        public void CancelSale(string identifier)
        {
            var sale = _saleRepository.GetSaleByIdentifier(identifier);

            if(sale == null)
                throw new SaleNotFoundException("Sale not found");

            bool isSaleWaitingForPaymentOrPaid = (sale.Status == EnumStatusSale.Waiting_payment || sale.Status == EnumStatusSale.Payment_accepted);
            if(!isSaleWaitingForPaymentOrPaid)
                throw new SaleServiceException($"Only sales with status {EnumStatusSale.Waiting_payment} or {EnumStatusSale.Payment_accepted} are cancellable");
            
            sale.Status = EnumStatusSale.Canceled;
            _saleRepository.UpdateSale(sale);
        }

        public void FinishSaleDelivery(string identifier)
        {
            var sale = _saleRepository.GetSaleByIdentifier(identifier);

            if(sale == null)
                throw new SaleNotFoundException("Sale not found");

            if(sale.Status != EnumStatusSale.Sent_to_carrier)
                throw new SaleServiceException($"Sale of identifier {identifier} was not sent to carrier");
            
            sale.Status = EnumStatusSale.Delivered;

            _saleRepository.UpdateSale(sale);
        }

        public Sale GetSaleByIdentifier(string identifier)
        {
            var sale = _saleRepository.GetSaleByIdentifier(identifier);

            return sale;
        }

        public void SendSaleToCarrier(string identifier)
        {
            var sale = _saleRepository.GetSaleByIdentifier(identifier);

            if(sale == null)
                throw new SaleNotFoundException("Sale not found");

            if(sale.Status != EnumStatusSale.Payment_accepted)
                throw new SaleServiceException($"Sale of idenfifier {identifier} is not with payment accepted");
            
            sale.Status = EnumStatusSale.Sent_to_carrier;

            _saleRepository.UpdateSale(sale);
        }
    }
}