using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EcommerceApp.Api.CustomFilters
{
    public class SkipCartOwnerCheckAttribute : Attribute { }
    public class ValidateCartOwnerFilterAttribute : IAsyncActionFilter
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<AppUser> userManager;

        public ValidateCartOwnerFilterAttribute(
            ICartRepository cartRepository,
            UserManager<AppUser> userManager)
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var skipAttribute = context.ActionDescriptor
                .EndpointMetadata
                .OfType<SkipCartOwnerCheckAttribute>()
                .SingleOrDefault();

            if (skipAttribute != null)
            {
                await next();
                return;
            }


            var user = await userManager.FindByNameAsync(
                context.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!context.ActionArguments.TryGetValue("cartId", out var cartIdObj))
            {
                context.Result = new BadRequestObjectResult("The request is not valid.");
                return;
            }

            string cartId = cartIdObj?.ToString() ?? string.Empty;

            var cartOwnerId = await cartRepository.GetCartOwnerIdAsync(cartId);

            if (user.Id != cartOwnerId)
            {
                context.Result = new BadRequestObjectResult("The user does not possess this cart.");
                return;
            }


            await next();
        }
    }
}
