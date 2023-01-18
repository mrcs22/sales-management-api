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
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<ISellerRepository> _mockSellerRepository;
        public SaleServiceTest()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockSellerRepository = new Mock<ISellerRepository>();
        }

        [Fact]
        public void ShouldReturnASaleWhenGetSaleByIdWithValidId()
        {
            Sale expectedSale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            _mockSaleRepository.Setup(s => s.GetSaleById(expectedSale.Id)).Returns(expectedSale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Sale sale = saleService.GetSaleById(expectedSale.Id);

            Assert.Equal(expectedSale, sale);
        }

        [Fact]
        public void ShouldReturnNullWhenGetSaleByIdWithInvalidId()
        {
            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            int invalidSaleId = 0;
            Sale sale = saleService.GetSaleById(invalidSaleId);

            Assert.Equal(null, sale);
        }

        [Fact]
        public void ShouldCreateASaleWithNoExceptionsWhenSellerIsNew()
        {
            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            var exception = Record.Exception(() =>
            {
                saleService.CreateSale(sale);
            });


            Assert.Null(exception);
        }

        [Fact]
        public void ShouldCreateASaleWithNoExceptionsWhenSellerExists()
        {
            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            _mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(sale.Seller.Cpf, sale.Seller.Email)).Returns(sale.Seller);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            var exception = Record.Exception(() =>
            {
                saleService.CreateSale(sale);
            });


            Assert.Null(exception);
        }

        [Fact]
        public void ShouldApproveSalePaymentForSaleWaitingPayment()
        {
            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Equal(EnumStatusSale.Waiting_payment, sale.Status);

            saleService.ApproveSalePayment(sale.Id);

            Assert.Equal(EnumStatusSale.Payment_accepted, sale.Status);
        }

        [Theory]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Delivered)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        [InlineData(EnumStatusSale.Sent_to_carrier)]
        public void ShouldThrowSaleServiceExceptionWhenApproveSalePaymentForSaleNotWaitingPayment(EnumStatusSale status)
        {
            Sale sale = SaleFactory.CreateValidSale(status);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.ApproveSalePayment(sale.Id));
        }

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        public void ShouldCancelSaleIfItIsWaitingPaymentOrPaid(EnumStatusSale status)
        {
            Sale sale = SaleFactory.CreateValidSale(status);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.NotEqual(EnumStatusSale.Canceled, sale.Status);

            saleService.CancelSale(sale.Id);

            Assert.Equal(EnumStatusSale.Canceled, sale.Status);
        }

        [Theory]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Delivered)]
        [InlineData(EnumStatusSale.Sent_to_carrier)]
        public void ShouldThrowSaleServiceExceptionWhenCancelSaleIfItIsNotWaitingPaymentOrPaid(EnumStatusSale status)
        {
            Sale sale = SaleFactory.CreateValidSale(status);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.CancelSale(sale.Id));
        }

        [Fact]
        public void ShouldFinishSaleDeliveryIfItWasSentToCarrier()
        {
            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Sent_to_carrier);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Equal(EnumStatusSale.Sent_to_carrier, sale.Status);

            saleService.FinishSaleDelivery(sale.Id);

            Assert.Equal(EnumStatusSale.Delivered, sale.Status);
        }

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Delivered)]

        public void ShouldThrowSaleServiceExceptionWhenFinishSaleDeliveryIfItWasNotSentToCarrier(EnumStatusSale status)
        {
            Sale sale = SaleFactory.CreateValidSale(status);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.FinishSaleDelivery(sale.Id));
        }

        [Fact]
        public void ShouldSendSaleToCarrierIfItIsPaid()
        {
            Sale sale = SaleFactory.CreateValidSale(EnumStatusSale.Payment_accepted);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Equal(EnumStatusSale.Payment_accepted, sale.Status);

            saleService.SendSaleToCarrier(sale.Id);

            Assert.Equal(EnumStatusSale.Sent_to_carrier, sale.Status);
        }

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Canceled)]
        [InlineData(EnumStatusSale.Sent_to_carrier)]
        [InlineData(EnumStatusSale.Delivered)]
        public void ShouldThrowSaleServiceExceptionWhenSendSaleToCarrierIfItIsNotPaid(EnumStatusSale status)
        {
            Sale sale = SaleFactory.CreateValidSale(status);
            _mockSaleRepository.Setup(s => s.GetSaleById(sale.Id)).Returns(sale);

            var saleService = new SaleService(_mockSaleRepository.Object, _mockSellerRepository.Object);

            Assert.Throws<SaleServiceException>(() => saleService.SendSaleToCarrier(sale.Id));
        }
    }
}