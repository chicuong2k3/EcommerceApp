namespace EcommerceApp.BlazorWeb.Requests;

public class UpdateCategoryRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

internal class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên danh mục.")
            .MaximumLength(100).WithMessage("Tên danh mục không được quá 100 kí tự.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
    async (requestModel, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UpdateCategoryRequest>
            .CreateWithOptions(
            requestModel as UpdateCategoryRequest,
            x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return [];
        }

        return result.Errors.Select(x => x.ErrorMessage);
    };
}
