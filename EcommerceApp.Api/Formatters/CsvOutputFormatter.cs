using EcommerceApp.Api.Dtos;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace EcommerceApp.Api.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(CategoryGetDto).IsAssignableFrom(type) || typeof(IEnumerable<CategoryGetDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        private static void FormatCsv(StringBuilder buffer, CategoryGetDto categoryGetDto)
        {
            buffer.AppendLine($"{categoryGetDto.Id},\"{categoryGetDto.Name}\"");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            var categories = context.Object as IEnumerable<CategoryGetDto>;
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    FormatCsv(buffer, category);
                }
            }
            else
            {
                FormatCsv(buffer, context.Object as CategoryGetDto ?? new CategoryGetDto());
            }

            await response.WriteAsync(buffer.ToString());
        }
    }
}