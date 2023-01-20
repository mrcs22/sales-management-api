using Moq;
using tech_test_payment_api.Repositories;
using tech_test_payment_api.Services;
using tech_test_payment_api.Models;
using tests.Factories;

namespace tests
{
    public class SellerServiceTest
    {
        private readonly Mock<ISellerRepository> _mockSellerRepository;

        public SellerServiceTest()
        {
            _mockSellerRepository = new Mock<ISellerRepository>();
        }

        [Fact]
        public void shouldReturnTrueWhenValidateSellerWhenAllSellerInfoMatch(){
            Seller seller = SellerFactory.CreateValidSeller();
            _mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(seller.Cpf,seller.Email)).Returns(seller);

            ISellerService sellerService = new SellerService(_mockSellerRepository.Object);
            bool isSellerValid = sellerService.ValidateSeller(seller);

            Assert.True(isSellerValid);
        }

        [Fact]
        public void shouldReturnTrueWhenValidateANewSeller(){
            Seller seller = SellerFactory.CreateValidSeller();

            ISellerService sellerService = new SellerService(_mockSellerRepository.Object);
            bool isSellerValid = sellerService.ValidateSeller(seller);

            Assert.True(isSellerValid);
        }

        [Fact]
        public void shouldReturnFalseWhenValidateSellerWhitWrongName(){
            Seller seller = SellerFactory.CreateValidSeller(name:"Test Seller Name");
            Seller sellerWrongName = SellerFactory.CreateValidSeller(name: "Wrong Test Seller Name");
            
            _mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(sellerWrongName.Cpf,sellerWrongName.Email)).Returns(seller);
           
            ISellerService sellerService = new SellerService(_mockSellerRepository.Object);
            
            bool isSellerValid = sellerService.ValidateSeller(sellerWrongName);

            Assert.False(isSellerValid);
        }   

        [Fact]
        public void shouldReturnFalseWhenValidateSellerWhitWrongCpf(){
            Seller seller = SellerFactory.CreateValidSeller(cpf: "01234567890"); 
            Seller sellerWrongCpf = SellerFactory.CreateValidSeller(cpf: "00000000000");
            
            _mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(sellerWrongCpf.Cpf,sellerWrongCpf.Email)).Returns(seller);
 
            ISellerService sellerService = new SellerService(_mockSellerRepository.Object);

            bool isSellerValid = sellerService.ValidateSeller(sellerWrongCpf);

            Assert.False(isSellerValid);
        }   

        [Fact]
        public void shouldReturnFalseWhenValidateSellerWhitWrongEmail(){
            Seller seller = SellerFactory.CreateValidSeller(email:"seler@test.com");
            Seller sellerWrongEmail = SellerFactory.CreateValidSeller(email:"wrong_seler@test.com");
            
            _mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(sellerWrongEmail.Cpf,sellerWrongEmail.Email)).Returns(seller);
 
            ISellerService sellerService = new SellerService(_mockSellerRepository.Object);
 
            bool isSellerValid = sellerService.ValidateSeller(sellerWrongEmail);

            Assert.False(isSellerValid);
        }   

        [Fact]
        public void shouldReturnFalseWhenValidateSellerWhitWrongPhoneNumber(){
            Seller seller = SellerFactory.CreateValidSeller(phoneNumber:"99999999999");
           Seller sellerWrongPhoneNumber = SellerFactory.CreateValidSeller(phoneNumber:"00000000000");
            
            _mockSellerRepository.Setup(s => s.GetSellerByCpfOrEmail(sellerWrongPhoneNumber.Cpf,sellerWrongPhoneNumber.Email)).Returns(seller);
 
            ISellerService sellerService = new SellerService(_mockSellerRepository.Object);

             bool isSellerValid = sellerService.ValidateSeller(sellerWrongPhoneNumber);

            Assert.False(isSellerValid);
        }        

    }
}