

namespace EcommerceApp.Common.Exceptions
{
    public class BadRequestException<TEntity> : Exception
    {
        public BadRequestException() : base($"Invalid parameter. Parameter type: {typeof(TEntity).Name}.")
        {
        }
    }
}
