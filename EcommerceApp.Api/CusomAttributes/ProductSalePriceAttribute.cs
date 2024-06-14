using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.CusomAttributes
{
    public class ProductSalePriceAttribute : ValidationAttribute
    {
        public ProductSalePriceAttribute()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var product = validationContext.ObjectInstance as ProductCreateUpdateDto;

            if (product == null || product.SalePrice > product.Price)
            {
                return new ValidationResult("The product's sale price must be less than its price.");
            }

            return ValidationResult.Success;
        }
    }
}