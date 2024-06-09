using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
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
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<CategoryGetDto>)
            {
                foreach (var category in (IEnumerable<CategoryGetDto>)context.Object)
                {
                    FormatCsv(buffer, category);
                }
            }            else
            {
                FormatCsv(buffer, (CategoryGetDto)context.Object);
            }

            await context.HttpContext.Response.WriteAsync(buffer.ToString());
        }
    }
}