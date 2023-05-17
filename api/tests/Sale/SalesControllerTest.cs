using Moq;
using tech_test_payment_api.Models;
using tech_test_payment_api.Services;
using tech_test_payment_api.Controllers;
using tests.Factories;
using Microsoft.AspNetCore.Mvc;

namespace tests
{
    public class SalesControllerTest
    {
        private readonly Mock<ISaleService> _mockSaleService;
        private readonly Mock<ISellerService> _mockSellerService;
        public SalesControllerTest()
        {
            _mockSaleService = new Mock<ISaleService>();
            
            _mockSellerService = new Mock<ISellerService>();
        }

        [Fact]
        public void ShouldReturnSaleWithOkStatusWhenGetSaleByIdWithValidIdentifier()
        {
            var expectedSale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            _mockSaleService.Setup(s => s.GetSaleByIdentifier(expectedSale.OrderIdentifier)).Returns(expectedSale);

            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var result = saleController.GetSaleByIdentifier(expectedSale.OrderIdentifier) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(expectedSale, result.Value);
        }

        [Fact]
        public void ShouldReturnNotFoundStatusWhenGetSaleByIdWithInvalidIdentifier()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            string invalidSaleIdentifier = "JustAText";
            var result = saleController.GetSaleByIdentifier(invalidSaleIdentifier) as NotFoundResult;

            Assert.NotNull(result);

            int notFoundStatusCode = 404;
            Assert.Equal(notFoundStatusCode, result.StatusCode);
        }

        [Fact]
        public void ShouldReturnSaleWithCreatedStatusWhenCreatingSale()
        {
            var validSaleBody = SaleFactory.CreateValidSaleBody();
            
            _mockSellerService.Setup(s => s.ValidateSeller(validSaleBody.Seller)).Returns(true);
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var result = saleController.CreateSale(validSaleBody) as CreatedAtActionResult;
            
            Assert.NotNull(result);
            
            int createdStatusCode = 201;
            Assert.Equal(createdStatusCode, result.StatusCode);
            
            Assert.Equal(validSaleBody, result.Value);
        }

        [Fact]
        public void ShouldReturnUnauthorizedStatusWhenCreatingSaleWithWrongSellerInfo()
        {
            var validSaleBody = SaleFactory.CreateValidSaleBody();
            
            _mockSellerService.Setup(s => s.ValidateSeller(validSaleBody.Seller)).Returns(false);
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var result = saleController.CreateSale(validSaleBody) as UnauthorizedObjectResult;
            
            Assert.NotNull(result);
            
            int unauthorizedStatusCode = 401;
            Assert.Equal(unauthorizedStatusCode, result.StatusCode);
        }

        [Fact]
        public void ShouldReturnOkStatusWhenApprovePaymentForSaleWaitingPayment()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            var result = saleController.ApproveSalePayment(sale.OrderIdentifier) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }

        [Theory]
        [InlineData(EnumStatusSale.Waiting_payment)]
        [InlineData(EnumStatusSale.Payment_accepted)]
        public void ShouldReturnOkStatusWhenCancelSaleForACancellableSale(EnumStatusSale status)
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(status);
            var result = saleController.CancelSale(sale.OrderIdentifier) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }        

        [Fact]
        public void ShouldReturnOkStatusWhenSendSaleToCarrierForAPaidSale()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Payment_accepted);
            var result = saleController.SendSaleToCarrier(sale.OrderIdentifier) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }

        [Fact]
        public void ShouldReturnOkStatusWhenFinishSaleDeliveryForASentSale()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Sent_to_carrier);
            var result = saleController.FinishSaleDelivery(sale.OrderIdentifier) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }
    }
}