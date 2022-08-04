using Auth.DTO.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.WebApi.Common
{
    public class PermissionRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerAllowAnonymous = context.Controller.GetType().GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
            var actionAllowAnonymous = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() ?? false;
            if (controllerAllowAnonymous || actionAllowAnonymous)
                return;

            if (!context.HttpContext.Request.Cookies.Any())
            {
                context.Result = !context.HttpContext.Request.Path.Value.ToLower().Contains("api") ?
                    new JsonResult(
                        new ResponseModel
                        {
                            Code = ResponseCode.UnAuth,
                            Message = "未授权"
                        }) :
                    new JsonResult(
                        new ResponseModel
                        {
                            Code = ResponseCode.UnLogin,
                            Message = "登录已过期"
                        });
            }

        }
    }
}
