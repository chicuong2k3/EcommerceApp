using EcommerceApp.Domain.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace EcommerceApp.Domain.Shared
{
    public static class LinqExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> list, string? orderQueryString)
        {
            if (string.IsNullOrEmpty(orderQueryString))
            {
                return list.OrderBy(x => x.Name);
            }



            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Product>(orderQueryString);

            if (string.IsNullOrEmpty(orderQuery))
            {
                return list.OrderBy(x => x.Name);
            }

            return list.OrderBy(orderQuery);
        }
    }
}
