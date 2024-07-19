namespace EcommerceApp.BlazorWeb.Responses
{
    internal class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

    }
}
