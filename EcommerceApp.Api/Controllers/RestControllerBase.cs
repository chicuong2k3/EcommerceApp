using AutoMapper;
using EcommerceApp.Api.HATEOAS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EcommerceApp.Api.Controllers
{
    [ApiController]
    public class RestControllerBase : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly ILinkService linkService;

        public RestControllerBase(IMapper mapper,
            ILinkService linkService)
        {
            this.mapper = mapper;
            this.linkService = linkService;
        }

        protected bool ShouldGenerateLinks()
        {
            var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hal", StringComparison.InvariantCultureIgnoreCase);

        }
    }
}
