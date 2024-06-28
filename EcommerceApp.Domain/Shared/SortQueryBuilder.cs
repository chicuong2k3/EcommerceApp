using EcommerceApp.Domain.Models;
using System.Reflection;
using System.Text;

namespace EcommerceApp.Domain.Shared
{
    public class SortQueryBuilder
    {
        public static string CreateSortQuery<T>(string raw)
        {
            var orderParams = raw.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var orderParam in orderParams)
            {
                if (string.IsNullOrEmpty(orderParam))
                    continue;

                var propName = orderParam.Split(" ")[0];
                var prop = propertyInfos.FirstOrDefault(x => x.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase));

                if (prop == null)
                    continue;

                var dir = orderParam.EndsWith("desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{prop.Name.ToString()} {dir}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
