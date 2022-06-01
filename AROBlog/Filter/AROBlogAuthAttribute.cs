using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AROBlog.Filter
{
    public class AROBlogAuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {

        //每个action执行之前都会进入这个方法
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //当用户存储在cookie中且session数据为空时，把cookie的数据同步到session中
            if (context.HttpContext.Request.Cookies["loginAccount"] != null &&
                context.HttpContext.Session.GetString("loginAccount") == null)
            {

                context.HttpContext.Session.SetString("loginAccount", context.HttpContext.Request.Cookies["loginAccount"].ToString());
                context.HttpContext.Session.SetString("userid_session", context.HttpContext.Request.Cookies["userId_cookie"].ToString());
            }


            //如果不通过认证 重定向到/Login/User页
            if (context.HttpContext.User.Identity.IsAuthenticated || HasAllowAnonymous(context) == true) return;
            context.Result = new RedirectToActionResult("Login", "User", null);
        }

        //用于判断Action有没有AllowAnonymous标签，微软写的
        private bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }

            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return true;
            }

            return false;
        }



    }
}
