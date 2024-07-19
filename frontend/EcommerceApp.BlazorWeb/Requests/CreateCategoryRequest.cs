namespace EcommerceApp.BlazorWeb.Requests;

public class CreateCategoryRequest
{
    public string Name { get; set; } = default!;
}

internal class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên danh mục.")
            .MaximumLength(100).WithMessage("Tên danh mục không được quá 100 kí tự.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
        async (requestModel, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<CreateCategoryRequest>
                .CreateWithOptions(
                requestModel as CreateCategoryRequest,
                x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
            {
                return [];
            }

            return result.Errors.Select(x => x.ErrorMessage);
        };

}