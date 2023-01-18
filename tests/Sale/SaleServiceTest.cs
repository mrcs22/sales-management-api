using Moq;
using tech_test_payment_api.Models;
using tech_test_payment_api.Repositories;
using tech_test_payment_api.Services;
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

    }
}