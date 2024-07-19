using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Common.CusomAttributes
{
    public class LessThanOrEqualAttribute : ValidationAttribute
    {
        private readonly string propertyToCompareName;
        public LessThanOrEqualAttribute(string propName)
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
                && comparableValue.CompareTo(comparableValueToCompare) > 0)
            {
                return new ValidationResult($"The {validationContext.DisplayName} must be less than or equal to {propertyToCompareName}.");
            }

            return ValidationResult.Success;
        }
    }
}