

namespace tech_test_payment_api.Services.Exceptions
{
    public class SaleNotFoundException : SaleServiceException
    {
        public SaleNotFoundException() : base()
        {}
        public SaleNotFoundException(string message) : base(message)
        {}        
    }
}