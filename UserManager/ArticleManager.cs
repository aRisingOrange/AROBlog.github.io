using AROBlog.DAL;
using AROBlog.Data.Models;
using AROBlog.DTO;
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
    public class ArticleManager : IArticleManager
    {
        /// <summary>
        /// 上传文档
        /// </summary>
        /// <param name="title"></param>
        /// <param name="path"></param>
        /// <param name="categoryIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task PostArticle(string title, string path, string summary, Guid[] categoryIds, Guid userId)
        {
            using (var articleSvc = new ArticleService())
            {
                var article = new Article()
                {
                    Title = title,
                    StoragePath = path,
                    UserId = userId,
                    Summary = summary,
                };
                await articleSvc.CreateAsync(article);
                Guid articleId = article.Id;
                using (var articleToCategorySvc = new ArticleToCategoryService())
                {
                    foreach (var categoryId in categoryIds)
                    {
                        await articleToCategorySvc.CreateAsync(new ArticleToCategory()
                        {
                            ArticleId = articleId,
                            CategoryId = categoryId,
                        }, saved: false);
                    }
                    await articleToCategorySvc.Save();
                }
            }

        }

        #region 创建文章-在线编辑（暂时弃用）
        /* public async Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId)
                 {

                     using (var articleSvc = new ArticleService())
                     {
                         var article = new Article()
                         {
                             Title = title,
                             Content = content,
                         };
                         await articleSvc.CreateAsync(article);

                         Guid articleId = article.Id;
                         using (var articleToCategorySvc = new ArticleToCategoryService())
                         {
                             foreach (var categoryId in categoryIds)
                             {
                                 await articleToCategorySvc.CreateAsync(new ArticleToCategory()
                                 {
                                     ArticleId = articleId,
                                     CategoryId = categoryId
                                 }, saved: false);
                             }
                             await articleToCategorySvc.Save();
                         }
                     }
                 }
                 */
        #endregion

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateCategory(string name, Guid userId)
        {
            using (var categorySvc = new CategoryService())
            {
                //给BlogCategory传入name，userId
                await categorySvc.CreateAsync(new Category()
                {
                    CategoryName = name,
                    UserId = userId
                });
            }
        }

        public Task EditArticle(Guid articleId, string title, string summary, Guid[] categoryIds)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 修改类别
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="newCategoryName"></param>
        /// <returns></returns>
        public async Task EditCategory(Guid categoryId, string newCategoryName)
        {
            using (ICategoryService categoryService = new CategoryService())
            {
                var category = await categoryService.GetOneByIdAsync(categoryId);
                category.CategoryName = newCategoryName;
                await categoryService.EditAsync(category);
            }
        }

        /// <summary>
        /// 根据id判断是否存在文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsArticle(Guid articleId)
        {
            using (IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync().AnyAsync(m => m.Id == articleId);
            }
        }
        /// <summary>
        /// 获取所有数据库中所有文章
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetAllArticles()
        {
            using (var articleSvc = new ArticleService())
            {
                var list = await articleSvc.GetAllOrderAsync().Select(m => new ArticleDTO()
                {
                    Id = m.Id,
                    Title = m.Title,
                    CreateTime = m.CreateTime,
                    StoragePath = m.StoragePath,
                    Summary = m.Summary,
                }).ToListAsync();
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var articleDto in list)
                    {
                        var cates = await articleToCategoryService.GetAllAsync().Include(m => m.Category).Where(m => m.ArticleId == articleDto.Id).ToListAsync();

                        articleDto.CategoryIds = cates.Select(m => m.CategoryId).ToArray();

                        articleDto.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();
                    }
                    return list;
                }

            }
        }
        /// <summary>
        /// 获取所有数据库中所有文章并分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetAllArticles(int pageIndex, int pageSize)
        {
            using (var articleSvc = new ArticleService())
            {
                var list = await articleSvc.GetAllByPageOrderAsync(pageSize, pageIndex, false)
                  .Select(m => new ArticleDTO()
                  {
                      Id = m.Id,
                      Title = m.Title,
                      CreateTime = m.CreateTime,
                      Summary = m.Summary,
                  }).ToListAsync();
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var articleDto in list)
                    {
                        var cates = await articleToCategoryService.GetAllAsync().Include(m => m.Category).Where(m => m.ArticleId == articleDto.Id).ToListAsync();

                        articleDto.CategoryIds = cates.Select(m => m.CategoryId).ToArray();

                        articleDto.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();
                    }
                    return list;
                }

            }
        }
        /// <summary>
        /// 根据用户id获取所有文章
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize)
        {
            using (var articleSvc = new ArticleService())
            {
                var list = await articleSvc.GetAllByPageOrderAsync(pageSize, pageIndex, false).Include(m => m.User).Where(m => m.UserId == userId)
                  .Select(m => new ArticleDTO()
                  {
                      Id = m.Id,
                      Title = m.Title,
                      CreateTime = m.CreateTime,
                      StoragePath = m.StoragePath,
                  }).ToListAsync();
                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    foreach (var articleDto in list)
                    {
                        var cates = await articleToCategoryService.GetAllAsync().Include(m => m.Category).Where(m => m.ArticleId == articleDto.Id).ToListAsync();

                        articleDto.CategoryIds = cates.Select(m => m.CategoryId).ToArray();

                        articleDto.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();
                    }
                    return list;
                }

            }
        }
        public async Task<List<ArticleDTO>> GetAllArticlesByCategoryId(Guid categoryId, int pageIndex, int pageSize)
        {
           using(var articleToCategoryService = new ArticleToCategoryService())
            {
                var articles=await articleToCategoryService.GetAllByPageOrderAsync(pageSize, pageIndex, false)
                    .Include(m => m.Article).Where(m=>m.CategoryId==categoryId)
                    .Select(m=>new ArticleDTO()
                    {
                        Id = m.ArticleId,
                        CreateTime = m.CreateTime,
                    }).ToListAsync();
                using (IArticleService articleService = new ArticleService())
                {
                    foreach (var article in articles)
                    {
                        var cates = await articleService.GetAllAsync().Include(m=>m.ArticleToCategories).Where(m => m.Id == article.Id).ToListAsync();
                        article.Title = cates.Select(m=>m.Title).First();
                        article.Summary = cates.Select(m => m.Summary).First();
                    }
                    return articles;
                }
            }
        }
        /// <summary>
        /// 根据用户id获取所有分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CategoryDTO>> GetAllCategories(Guid userId)
        {
            using (ICategoryService categoryService = new CategoryService())
            {
                return await categoryService.GetAllAsync().Where(m => m.UserId == userId).Select(m => new CategoryDTO()
                {
                    Id = m.Id,
                    CategoryName = m.CategoryName
                }).ToListAsync();
            }
        }
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            using (ICategoryService categoryService = new CategoryService())
            {
                return await categoryService.GetAllAsync().Select(m => new CategoryDTO()
                {
                    Id = m.Id,
                    CategoryName = m.CategoryName
                }).ToListAsync();
            }
        }


        ///// <summary>
        ///// 获取本用户发布文章的列表总页数
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async Task<int> GetDataCount(Guid userId)
        //{
        //    using (IDAL.IArticleService articleService = new ArticleService())
        //    {
        //        return await articleService.GetAllAsync().CountAsync(m => m.UserId == userId);
        //    }
        //}
        public async Task<int> GetDataCount(Guid categoryId)
        {
            using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
            {
                return await articleToCategoryService.GetAllAsync().CountAsync(m => m.CategoryId == categoryId);
            }
        }
        /// <summary>
        /// 获取所有的文章总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetDataCount()
        {
            using (IDAL.IArticleService articleService = new ArticleService())
            {
                return await articleService.GetAllAsync().CountAsync();
            }
        }
        /// <summary>
        /// 通过id获取一篇文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ArticleDTO> GetOneArticleById(Guid articleId)
        {
            using (IArticleService articleService = new ArticleService())
            {
                var data = await articleService.GetAllAsync()
                    .Include(m => m.User)
                    .Where(m => m.Id == articleId)
                    .Select(m => new ArticleDTO()
                    {
                        Id = m.Id,
                        Title = m.Title,
                        CreateTime = m.CreateTime,
                        StoragePath = m.StoragePath,
                    }).FirstAsync();

                //根据存储的地址读取md文档
                StreamReader tempContent = new StreamReader(data
                    .StoragePath);
                var content = tempContent.ReadToEnd();
                tempContent.Close();
                tempContent.Dispose();
                data.Content = content;


                using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
                {
                    var cates = await articleToCategoryService.GetAllAsync().Include(m => m.Category)
                        .Where(m => m.ArticleId == data.Id).ToListAsync();
                    data.CategoryIds = cates.Select(m => m.CategoryId).ToArray();
                    data.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();

                    return data;
                }
            }
        }

        public async Task<CategoryDTO> GetOneCategoryById(Guid categoryId)
        {
            using (ICategoryService categoryService = new CategoryService())
            {
                var data = await categoryService.GetAllAsync()
                   .Include(m => m.User)
                   .Where(m => m.Id == categoryId)
                   .Select(m => new CategoryDTO()
                   {
                       Id = m.Id,
                       CategoryName = m.CategoryName,
                   }).FirstAsync();

                return data;
            }
        }

        /// <summary>
        /// 根据文章id删除文章（删除数据库内容与文件夹文件）
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task RemoveArticle(Guid articleId)
        { 
            //using (IArticleToCategoryService articleToCategoryService = new ArticleToCategoryService())
            //{
            //    //var data = await articleToCategoryService.GetAllAsync()
            //    //   .Include(m => m.Category)
            //    //   .Where(m => m.ArticleId == articleId)
            //    //   .FirstAsync();
            //    await articleToCategoryService.RemoveAsync(articleId, true);
            //}

            using (IArticleService articleService = new ArticleService())
            {
                var data = await articleService.GetAllAsync()
                    .Include(m => m.User)
                    .Where(m => m.Id == articleId)
                    .Select(m => new ArticleDTO()
                    {
                        StoragePath = m.StoragePath,
                    }).FirstAsync();
                System.IO.File.Delete(data.StoragePath);
                await articleService.RemoveAsync(articleId, true);
             
            }
           
        }
        /// <summary>
        /// 根据类别id删除类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveCategory(Guid id)
        {

            using (ICategoryService categoryService = new CategoryService())
            {
                var data = await categoryService.GetOneByIdAsync(id);

                await categoryService.RemoveAsync(data.Id);
            }
        }
    }
}
