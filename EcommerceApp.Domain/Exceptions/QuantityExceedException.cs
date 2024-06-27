namespace EcommerceApp.Domain.Exceptions
{
    public class QuantityExceedException : Exception
    {
        public QuantityExceedException()
        {
            
        }

        public QuantityExceedException(string message) : base(message) 
        {
            
        }
    }
}
