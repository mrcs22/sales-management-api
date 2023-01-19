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
        [Route("{id}")]
        public IActionResult GetSaleById(int id){
           var sale = _saleService.GetSaleById(id);

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

            return CreatedAtAction(nameof(GetSaleById), new {id = sale.Id}, sale);
        }

        [HttpPost]
        [Route("{id}/approve-payment")]
        public IActionResult ApproveSalePayment(int id){
            _saleService.ApproveSalePayment(id);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/cancel")]
        public IActionResult CancelSale(int id){
            _saleService.CancelSale(id);

            return Ok();           
        }

        [HttpPost]
        [Route("{id}/mark-as-sent")]
        public IActionResult SendSaleToCarrier(int id){
            _saleService.SendSaleToCarrier(id);

            return Ok();
           
        }        

        [HttpPost]
        [Route("{id}/mark-as-delivered")]
        public IActionResult FinishSaleDelivery(int id){
            _saleService.FinishSaleDelivery(id);

            return Ok();
        }  
       
    }
}