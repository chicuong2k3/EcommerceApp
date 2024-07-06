using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.CusomAttributes
{
    public class LessThanAttribute : ValidationAttribute
    {
        private readonly string propertyToCompareName;
        public LessThanAttribute(string propName)
        {
            propertyToCompareName = propName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var propertyToCompare = validationContext.ObjectType.GetProperty(propertyToCompareName);

            if (propertyToCompare == null)
            {
                return ValidationResult.Success;
            }

            var propertyToCompareValue = propertyToCompare.GetValue(validationContext.ObjectInstance, null);
            if (propertyToCompareValue == null || value == null)
            {
                return ValidationResult.Success;
            }

            var comparableValue = value as IComparable;
            var comparableValueToCompare = propertyToCompareValue as IComparable;

            if (comparableValue != null && comparableValueToCompare != null
                && comparableValue.CompareTo(comparableValueToCompare) >= 0)
            {
                return new ValidationResult($"The {validationContext.DisplayName} must be less than {propertyToCompareName}.");
            }

            return ValidationResult.Success;
        }
    }
}