

namespace tech_test_payment_api.Services.Exceptions
{
    public class SaleServiceException : Exception
    {
        public SaleServiceException() : base()
        {}
        public SaleServiceException(string message) : base(message)
        {}        
    }
}