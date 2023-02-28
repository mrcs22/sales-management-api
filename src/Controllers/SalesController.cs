using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Services;
using tech_test_payment_api.Models;
 namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ISellerService _sellerService;

        public SalesController(ISaleService saleService, ISellerService sellerService)
        {
            _saleService = saleService;
            _sellerService = sellerService;
        }

        [HttpGet]
        [Route("{identifier}")]
        public IActionResult GetSaleByIdentifier(string identifier){
           var sale = _saleService.GetSaleByIdentifier(identifier);

           if(sale == null)
            return NotFound();

           return Ok(sale);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateSale(Sale sale){
            bool isSellerValid = _sellerService.ValidateSeller(sale.Seller);
            if(!isSellerValid)
                return Unauthorized("Email e/ou Cpf já pertencem a um usuário. Preencha todos os dados do usuário corretamente para registrar a venda.");

            _saleService.CreateSale(sale);

            return CreatedAtAction(nameof(GetSaleByIdentifier), new {identifier = sale.OrderIdentifier}, sale);
        }

        [HttpPost]
        [Route("{identifier}/approve-payment")]
        public IActionResult ApproveSalePayment(string identifier){
            _saleService.ApproveSalePayment(identifier);

            return Ok();
        }

        [HttpPost]
        [Route("{identifier}/cancel")]
        public IActionResult CancelSale(string identifier){
            _saleService.CancelSale(identifier);

            return Ok();           
        }

        [HttpPost]
        [Route("{identifier}/mark-as-sent")]
        public IActionResult SendSaleToCarrier(string identifier){
            _saleService.SendSaleToCarrier(identifier);

            return Ok();
           
        }        

        [HttpPost]
        [Route("{identifier}/mark-as-delivered")]
        public IActionResult FinishSaleDelivery(string identifier){
            _saleService.FinishSaleDelivery(identifier);

            return Ok();
        }  
       
    }
}