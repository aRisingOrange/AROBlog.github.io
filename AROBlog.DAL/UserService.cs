using AROBlog.Data.Models;
using AROBlog.IDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.DAL
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService() : base(new BlogContext(new DbContextOptions<BlogContext>()))
        {
        }
    }
}
