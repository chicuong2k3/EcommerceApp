using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EcommerceApp.Api.CustomFilters
{
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

            var user = await userManager.FindByNameAsync(
                context.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty);

            var cartId = context.ActionArguments.FirstOrDefault(x => x.Key == "cartId").Value;

            if (!Guid.TryParse(cartId?.ToString(), out var result))
            {
                context.Result = new BadRequestObjectResult("The request is not valid.");
                return;
            }

            var cartOwnerId = await cartRepository.GetCartOwnerIdAsync(result);

            if (user == null || user.Id != cartOwnerId)
            {
                context.Result = new BadRequestObjectResult("The user does not possess this cart.");
                return;
            }


            await next();
        }
    }
}
