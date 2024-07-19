namespace EcommerceApp.BlazorWeb.Requests;

public class ProductVariationRequest
{
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
}

public class ProductItemRequest
{
    public int QuantityInStock { get; set; }
    public decimal BasePrice { get; set; }
    public List<ProductVariationRequest> ProductVariations { get; set; } = new();
}

public class CreateProductRequest
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<ProductItemRequest> ProductItems { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();

}

internal class ProductVariationValidator : AbstractValidator<ProductVariationRequest>
{
    public ProductVariationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên phân loại.")
            .MaximumLength(50).WithMessage("Tên phân loại không được quá 50 kí tự.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Vui lòng nhập giá trị phân loại.")
            .MaximumLength(50).WithMessage("Giá trị phân loại không được quá 50 kí tự.");
    }
}

internal class ProductItemValidator : AbstractValidator<ProductItemRequest>
{
    public ProductItemValidator()
    {
        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0).WithMessage("Số lượng trong kho phải lớn hơn hoặc bằng 0.");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0.");

        RuleForEach(x => x.ProductVariations)
            .SetValidator(new ProductVariationValidator());
    }
}

internal class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên sản phẩm.")
            .MaximumLength(100).WithMessage("Tên sản phẩm không được quá 100 kí tự.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Vui lòng nhập mô tả cho sản phẩm.")
            .MaximumLength(1000).WithMessage("Mô tả sản phẩm không được quá 1000 kí tự.");

        RuleFor(x => x.CategoryIds)
            .NotNull().WithMessage("Vui lòng chọn danh mục.");

        RuleForEach(x => x.ProductItems)
                .SetValidator(new ProductItemValidator());
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
        async (requestModel, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<CreateProductRequest>
                .CreateWithOptions(
                requestModel as CreateProductRequest,
                x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
            {
                return [];
            }

            return result.Errors.Select(x => x.ErrorMessage);
        };

}