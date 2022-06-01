using AROBlog.BLL;
using AROBlog.Filter;
using AROBlog.IBLL;
using AROBlog.Models.UserViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace AROBlog.Controllers
{
    public class UserController : Controller
    {
       
    [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //ModelState.IsValid指示是否可以将请求中的传入值正确绑定到模型，以及在模型绑定过程中是否违反了任何明确指定的验证规则。
            if (!ModelState.IsValid)
            {
                //不满足要求则返回窗口
                return View(model);
            }
            //业务逻辑层IUserManager接口
            IUserManager userManager = new UserManager();
            //为数据访问层传入邮箱和密码，注册用户，存入数据库
            await userManager.Register(model.Account, model.Password);
            return Content("注册成功");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IUserManager userManager = new UserManager();

                Guid userid;
                if (userManager.Login(model.LoginAccount, model.LoginPwd, out userid))
                {
                    //跳转
                    //判断使用cookies还是session
                    if (model.RememberMe)
                    {
                        CookieOptions options = new CookieOptions();
                        options.Expires = DateTime.Now.AddDays(7);

                        Response.Cookies.Append("loginAccount", model.LoginAccount, options);
                        Response.Cookies.Append("userId_cookie", userid.ToString(), options);
                    }
                    else
                    {
                        HttpContext.Session.SetString("loginAccount", model.LoginAccount);
                        HttpContext.Session.SetString("userid_session", userid.ToString());
                    }
                    #region Authrization,cookie验证
                    var identity = new ClaimsIdentity(new Claim[] { new Claim("loginAccount", model.LoginAccount) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var user = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(user);
                    #endregion
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "您的账号密码有误");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AROBlogAuth]
        public ActionResult ChangePwd()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePwd(ChangePwdViewModel model)
        {
            //ModelState.IsValid指示是否可以将请求中的传入值正确绑定到模型，以及在模型绑定过程中是否违反了任何明确指定的验证规则。
            if (!ModelState.IsValid)
            {
                //不满足要求则返回窗口
                return View(model);
            }
            //业务逻辑层IUserManager接口
            IUserManager userManager = new UserManager();
            //为数据访问层传入邮箱和旧密码、新密码，存入数据库
            await userManager.ChangePassword(model.Account,model.OldPwd,model.NewPwd);
            return Content("修改成功");
        }
    }
}
