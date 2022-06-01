using AROBlog.DAL;
using AROBlog.Data.Models;
using AROBlog.IBLL;
using AROBlog.IDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.BLL
{
    public class UserManager : IUserManager
    {
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task ChangePassword(string account, string oldPassword, string newPassword)
        {
            using (IUserService userSvc = new UserService())
            {
                if (await userSvc.GetAllAsync().AnyAsync(m => m.Account == account && m.Password == oldPassword))
                {
                    var user = await userSvc.GetAllAsync().FirstAsync(m => m.Account == account);
                    user.Password = newPassword;
                    await userSvc.EditAsync(user);
                }
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool Login(string account, string password, out Guid userid)
        {
            using (IUserService userSvc=new UserService())
            {
                var user = userSvc.GetAllAsync().FirstOrDefaultAsync(m => m.Account
                == account && m.Password == password);
                user.Wait();
                var data = user.Result;
                if(data==null)
                {
                    userid = new Guid();
                    return false;
                }
                else
                {
                    userid = data.Id;
                    return true;
                }
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task Register(string account, string password)
        {
            using (IUserService userSvc=new UserService())
            {
                await userSvc.CreateAsync(new User()
                {
                    Account = account,
                    Password = password,
                });
            }
        }
    }
}
