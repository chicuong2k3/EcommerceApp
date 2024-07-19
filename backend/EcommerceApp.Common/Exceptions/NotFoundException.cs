namespace EcommerceApp.Common.Exceptions
{
    public class NotFoundException<TEntity, TKey> : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(TKey id) : base($"{typeof(TEntity).Name} with id={id} not found.")
        {
        }
    }
}