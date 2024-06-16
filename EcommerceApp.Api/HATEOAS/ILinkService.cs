namespace EcommerceApp.Api.HATEOAS
{
    public interface ILinkService
    {
        Link GenerateLink(string endpointName, object? routeValues, string rel, string method);
    }
}
