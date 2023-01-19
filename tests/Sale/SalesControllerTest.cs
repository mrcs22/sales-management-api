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
        public SalesControllerTest()
        {
            _mockSaleService = new Mock<ISaleService>();
        }

        [Fact]
        public void ShouldReturnSaleWithOkStatusWhenGetSaleByIdWithValidId()
        {
            var expectedSale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            _mockSaleService.Setup(s => s.GetSaleById(expectedSale.Id)).Returns(expectedSale);

            var saleController = new SalesController(_mockSaleService.Object);

            var result = saleController.GetSaleById(expectedSale.Id) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(expectedSale, result.Value);
        }

        [Fact]
        public void ShouldReturnNotFoundStatusWhenGetSaleByIdWithInvalidId()
        {
            var saleController = new SalesController(_mockSaleService.Object);

            int invalidSaleId = 0;
            var result = saleController.GetSaleById(invalidSaleId) as NotFoundResult;

            Assert.NotNull(result);

            int notFoundStatusCode = 404;
            Assert.Equal(notFoundStatusCode, result.StatusCode);
        }

        [Fact]
        public void ShouldReturnSaleWithCreatedStatusWhenCreatingSale()
        {
            var expectedSale = SaleFactory.CreateValidSale(EnumStatusSale.Waiting_payment);
            expectedSale.Seller.PhoneNumber="55";
            _mockSaleService.Setup(s => s.GetSaleById(expectedSale.Id)).Returns(expectedSale);

            var saleController = new SalesController(_mockSaleService.Object);

            var result = saleController.CreateSale(expectedSale) as CreatedAtActionResult;
            
            Assert.NotNull(result);
            
            int createdStatusCode = 201;
            Assert.Equal(createdStatusCode, result.StatusCode);
            
            Assert.Equal(expectedSale, result.Value);
        }
    }
}