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
        public void ShouldReturnSaleWithOkStatusWhenGetSaleByIdWithValidId()
        {
            var expectedSale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            _mockSaleService.Setup(s => s.GetSaleById(expectedSale.Id)).Returns(expectedSale);

            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var result = saleController.GetSaleById(expectedSale.Id) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(expectedSale, result.Value);
        }

        [Fact]
        public void ShouldReturnNotFoundStatusWhenGetSaleByIdWithInvalidId()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            int invalidSaleId = 0;
            var result = saleController.GetSaleById(invalidSaleId) as NotFoundResult;

            Assert.NotNull(result);

            int notFoundStatusCode = 404;
            Assert.Equal(notFoundStatusCode, result.StatusCode);
        }

        [Fact]
        public void ShouldReturnSaleWithCreatedStatusWhenCreatingSale()
        {
            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            var seller = sale.Seller;
            var firstSaleProduct = sale.Products[0];

            var validSaleBody = new Sale {
                Seller = new Seller {
                        Name = seller.Name,
                        Cpf= seller.Cpf,
                        Email= seller.Email,
                        PhoneNumber= seller.PhoneNumber
                    },                     
                Products = new List<Product> {
                    new Product {      
                        Name = firstSaleProduct.Name,
                        Amount = firstSaleProduct.Amount,
                        Price = firstSaleProduct.Price    
                    }
                }
            };
            
            _mockSellerService.Setup(s => s.ValidateSeller(validSaleBody.Seller)).Returns(true);
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var result = saleController.CreateSale(validSaleBody) as CreatedAtActionResult;
            
            Assert.NotNull(result);
            
            int createdStatusCode = 201;
            Assert.Equal(createdStatusCode, result.StatusCode);
            
            Assert.Equal(validSaleBody, result.Value);
        }

        [Fact]
        public void ShouldReturnOkStatusWhenApprovePaymentForSaleWaitingPayment()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            var result = saleController.ApproveSalePayment(sale.Id) as OkResult;

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
            var result = saleController.CancelSale(sale.Id) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }        

        [Fact]
        public void ShouldReturnOkStatusWhenSendSaleToCarrierForAPaidSale()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Payment_accepted);
            var result = saleController.SendSaleToCarrier(sale.Id) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }

        [Fact]
        public void ShouldReturnOkStatusWhenFinishSaleDeliveryForASentSale()
        {
            var saleController = new SalesController(_mockSaleService.Object, _mockSellerService.Object);

            var sale = SaleFactory.CreateValidSale(EnumStatusSale.Sent_to_carrier);
            var result = saleController.FinishSaleDelivery(sale.Id) as OkResult;

            Assert.NotNull(result);

            int OkStatusCode = 200;
            Assert.Equal(OkStatusCode, result.StatusCode);
        }
    }
}