using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApp.Api.CustomFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor != null)
            {
                var parameters = actionDescriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters)
                {
                    var parameterName = parameter.Name ?? string.Empty;
                    if (context.ActionArguments.TryGetValue(parameterName, out var parameterValue))
                    {
                        if (parameterValue == null || IsDefaultValue(parameterValue, parameter.ParameterType))
                        {
                            context.Result = new BadRequestObjectResult($"The request is not valid.");
                            return;
                        }
                    }
                    
                }
            }


            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }

        private bool IsDefaultValue(object value, Type type)
        {
            if (type.IsValueType)
            {
                return value.Equals(Activator.CreateInstance(type));
            }

            return ReferenceEquals(value, null);
        }
    }
}
