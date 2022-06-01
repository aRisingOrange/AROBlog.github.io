using AROBlog.Data.Models;
using AROBlog.IDAL;
using Microsoft.EntityFrameworkCore;

namespace AROBlog.DAL
{
    public class ArticleService :BaseService<Article>,IArticleService
    {
        public ArticleService() : base(new BlogContext(new DbContextOptions<BlogContext>()))
        {
        }
    }
}
