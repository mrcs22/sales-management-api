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

           return Ok(sale);
        }
       
    }
}