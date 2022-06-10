using AROBlog.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.IBLL
{
    public  interface IArticleManager
    {
        Task PostArticle(string title, string path, string summary, Guid[] categoryIds, Guid userId);

        //Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId);

        Task CreateCategory(string name, Guid userId);

        Task<List<CategoryDTO>> GetAllCategories(Guid userId);
        Task<List<CategoryDTO>> GetAllCategories();
        Task<List<ArticleDTO>> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize);
        Task<List<ArticleDTO>> GetAllArticles();
        /// <summary>
        /// 获取总页码数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //Task<int> GetDataCount(Guid userId);
        Task<int> GetDataCount(Guid categoryId);
        Task<int> GetDataCount();

        //Task<List<ArticleDTO>> GetAllArticlesByAccount(string account);

        Task<List<ArticleDTO>> GetAllArticlesByCategoryId(Guid categoryId, int pageIndex, int pageSize);

        Task RemoveCategory(Guid id);

        Task EditCategory(Guid categoryId, string newCategoryName);

        Task RemoveArticle(Guid articleId);

        Task EditArticle(Guid articleId, string title, string summary, Guid[] categoryIds);

        //传入id判断是否存在文章
        Task<bool> ExistsArticle(Guid articleId);
        //通过id获取文章
        Task<ArticleDTO> GetOneArticleById(Guid articleId);

        Task<CategoryDTO> GetOneCategoryById(Guid categoryId);
    }
}
