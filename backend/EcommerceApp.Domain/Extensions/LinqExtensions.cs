using EcommerceApp.Domain.Models;
using System.Linq.Dynamic.Core;

namespace EcommerceApp.Common.Shared.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> list, string? orderQueryString)
        {
            if (string.IsNullOrEmpty(orderQueryString))
            {
                return list.OrderBy("Name");
            }

            var orderQuery = SortQueryBuilder.CreateSortQuery<Product>(orderQueryString);

            if (string.IsNullOrEmpty(orderQuery))
            {
                return list.OrderBy("Name");
            }

            return list.OrderBy(orderQuery);
        }
    }
}
