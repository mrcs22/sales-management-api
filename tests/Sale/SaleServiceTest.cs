using Moq;
using tech_test_payment_api.Models;
using tech_test_payment_api.Repositories;
using tech_test_payment_api.Services;
using tech_test_payment_api.Services.Exceptions;
using tests.Factories;

namespace tests 
{
    public class SaleServiceTest
    {
        [Fact]
        public void ShouldReturnASaleWhenGetSaleByIdWithValidId()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale expectedSale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(expectedSale);  

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Sale sale = saleService.GetSaleById(1);


            Assert.Equal(expectedSale, sale);    
        }

        [Fact]
        public void ShouldReturnNullWhenGetSaleByIdWithInvalidId()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Sale sale = saleService.GetSaleById(1);

            Assert.Equal(null, sale);    
        }

        [Fact]
        public void ShouldCreateASaleWithNoExceptionsWhenSellerIsNew()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

           var exception = Record.Exception(() => {
                saleService.CreateSale(sale);
            });
          

            Assert.Null(exception);    
        }

        [Fact]
        public void ShouldCreateASaleWithNoExceptionsWhenSellerExists()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(sale.Seller.Cpf, sale.Seller.Email)).Returns(sale.Seller);

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

           var exception = Record.Exception(() => {
                saleService.CreateSale(sale);
            });
          

            Assert.Null(exception);    
        }      

        [Fact]
        public void ShouldApproveSalePaymentForSaleWaitingPayment()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Equal(EnumStatusSale.Waiting_payment, sale.Status);  
            
            saleService.ApproveSalePayment(1);          

            Assert.Equal(EnumStatusSale.Payment_accepted, sale.Status);  
        }  

        [Theory]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Delivered)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        [InlineData(EnumStatusSale.Sent_to_carrier)]
        public void ShouldThrowSaleServiceExceptionWhenApproveSalePaymentForSaleNotWaitingPayment(EnumStatusSale status)
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(status);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);
           
            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.ApproveSalePayment(1));
        }  

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        public void ShouldCancelSaleIfItIsWaitingPaymentOrPaid(EnumStatusSale status)
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(status);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.NotEqual(EnumStatusSale.Canceled, sale.Status);  
            
            saleService.CancelSale(1);          

            Assert.Equal(EnumStatusSale.Canceled, sale.Status);  
        }   

        [Theory]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Delivered)]
        [InlineData(EnumStatusSale.Sent_to_carrier)]
        public void ShouldThrowSaleServiceExceptionWhenCancelSaleIfItIsNotWaitingPaymentOrPaid(EnumStatusSale status)
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(status);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);
           
            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.CancelSale(1));
        }         
  
        [Fact]
        public void ShouldFinishSaleDeliveryIfItWasSentToCarrier()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Sent_to_carrier);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Equal(EnumStatusSale.Sent_to_carrier, sale.Status);  
            
            saleService.FinishSaleDelivery(1);          

            Assert.Equal(EnumStatusSale.Delivered, sale.Status);  
        }   

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Delivered)]
       
        public void ShouldThrowSaleServiceExceptionWhenFinishSaleDeliveryIfItWasNotSentToCarrier(EnumStatusSale status)
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(status);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);
           
            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.FinishSaleDelivery(1));
        }   

        [Fact]
        public void ShouldSendSaleToCarrierIfItIsPaid()
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Payment_accepted);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);

            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Equal(EnumStatusSale.Payment_accepted, sale.Status);  
            
            saleService.SendSaleToCarrier(1);          

            Assert.Equal(EnumStatusSale.Sent_to_carrier, sale.Status);  
        }  

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Sent_to_carrier)]
        [InlineData(EnumStatusSale.Delivered)]
        public void ShouldThrowSaleServiceExceptionWhenSendSaleToCarrierIfItIsNotPaid(EnumStatusSale status)
        {
            var mockSaleRepository = new Mock<ISaleRepository>();
            var mockSellerRepository = new Mock<ISellerRepository>();

            Sale sale = SaleFactory.CreateValidSale(status);
            mockSaleRepository.Setup(s => s.GetSaleById(1)).Returns(sale);
           
            var saleService = new SaleService(mockSaleRepository.Object, mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.SendSaleToCarrier(1));
        } 
    }
}