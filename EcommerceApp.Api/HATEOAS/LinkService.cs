namespace EcommerceApp.Api.HATEOAS
{
    public class LinkService : ILinkService
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            this.linkGenerator = linkGenerator;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Link GenerateLink(string endpointName, object? routeValues, string rel, string method)
        {
            var href = linkGenerator.GetUriByName(httpContextAccessor.HttpContext, endpointName, routeValues);
            return new Link(href, rel, method);
        }
    }
}
