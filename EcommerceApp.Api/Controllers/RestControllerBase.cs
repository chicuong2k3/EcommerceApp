using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.HATEOAS;
using EcommerceApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EcommerceApp.Api.Controllers
{
    [ApiController]
    public class RestControllerBase : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly ILoggerService logger;
        protected readonly ILinkService linkService;

        public RestControllerBase(IMapper mapper,
            ILoggerService logger,
            ILinkService linkService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.linkService = linkService;
        }

        protected bool ShouldGenerateLinks()
        {
            var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hal", StringComparison.InvariantCultureIgnoreCase);

        }
    }
}
