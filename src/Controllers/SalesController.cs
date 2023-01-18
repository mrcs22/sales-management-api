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

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
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