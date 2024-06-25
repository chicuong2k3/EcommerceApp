using EcommerceApp.Domain.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace EcommerceApp.Domain.Shared
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> list, string? orderQueryString)
        {
            if (string.IsNullOrEmpty(orderQueryString))
            {
                return list.OrderBy("Name");
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Product>(orderQueryString);

            if (string.IsNullOrEmpty(orderQuery))
            {
                return list.OrderBy("Name");
            }

            return list.OrderBy(orderQuery);
        }
    }
}
