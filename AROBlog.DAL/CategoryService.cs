using AROBlog.Data.Models;
using AROBlog.IDAL;
using Microsoft.EntityFrameworkCore;

namespace AROBlog.DAL
{
    public class CategoryService  :BaseService<Category>,ICategoryService
    {
        public CategoryService() : base(new BlogContext(new DbContextOptions<BlogContext>()))
        {
        }
    }
}
