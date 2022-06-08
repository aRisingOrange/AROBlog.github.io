using AROBlog.BLL;
using AROBlog.DAL;
using AROBlog.DTO;
using AROBlog.Filter;
using AROBlog.IBLL;
using AROBlog.IDAL;
using AROBlog.Models.BlogViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AROBlog.Controllers
{
    [AROBlogAuth]
    public class ArticleController : Controller
    {
        private IWebHostEnvironment _env;
        private string _dir;
        public ArticleController(IWebHostEnvironment env)
        {
            _env = env;
            _dir = _env.WebRootPath;
        }
        #region Article文章
        /// <summary>
        /// 上传文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PostArticle()
        {
            var userid = HttpContext.Session.GetString("userid_session");
            Guid guid = new Guid(userid);
            ViewBag.CategoryIds = await new ArticleManager().GetAllCategories(guid);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> PostArticle(CreateArticleViewModel model, IFormFile file)   /*这里声明的file要对应视图页面input标签的name*/
        {
            if (ModelState.IsValid)
            {
                string savePath = _dir + "\\storage\\Article\\docs\\" + file.FileName;
                var userid = HttpContext.Session.GetString("userid_session");
                Guid guid = new Guid(userid);
                await new ArticleManager().PostArticle(file.FileName, savePath, model.Summary, model.CategoryIds, guid);
                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);

                }
                return RedirectToAction("ArticleList");

            }
            ModelState.AddModelError("", "上传失败");
            return View();
        }
        /// <summary>
        /// 文章列表后台
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ArticleListAdmin(int pageIndex = 0, int pageSize = 1)
        {

            //需要给页面前端 总页码数，当前页码，可显示的总页码数量
            var articleMgr = new ArticleManager();
            var userid = HttpContext.Session.GetString("userid_session");
            Guid guid = new Guid(userid);

            var articles = await articleMgr.GetAllArticlesByUserId(guid, pageIndex, pageSize);
            var dataCount = await articleMgr.GetDataCount(guid);
            ViewBag.PageCount = dataCount % pageSize == 0 ? dataCount / pageSize : dataCount / pageSize + 1;
            ViewBag.PageIndex = pageIndex;
            return View(articles);
        }
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ArticleList(int pageIndex = 0, int pageSize = 7)
        {
            ArticleViewModel model = new ArticleViewModel();
            //需要给页面前端 总页码数，当前页码，可显示的总页码数量
            var articleMgr = new ArticleManager();
            var userid = HttpContext.Session.GetString("userid_session");
            Guid guid = new Guid(userid);
            
            var articles = await articleMgr.GetAllArticles( pageIndex, pageSize);
            var dataCount = await articleMgr.GetDataCount(guid);
            var categories = await articleMgr.GetAllCategories();
            ViewBag.PageCount = dataCount % pageSize == 0 ? dataCount / pageSize : dataCount / pageSize + 1;
            ViewBag.PageIndex = pageIndex;

            model.articleDTOs = articles;
            model.categoryDTOs = categories;
            return View(model);
        }
        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ArticleDetails(Guid? id)
        {
            var articleMgr = new ArticleManager();
            if (id == null || !await articleMgr.ExistsArticle(id.Value))
                return RedirectToAction(nameof(ArticleList));
            return View(await articleMgr.GetOneArticleById(id.Value));
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteArticle(Guid id)
        {
            IArticleManager articleManager = new ArticleManager();
            await articleManager.RemoveArticle(id);
            return RedirectToAction("ArticleList");
        }
        #endregion
        #region Category分类
        /// <summary>
        /// 创建分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AROBlogAuth]
        public ActionResult CreateCategory(CreateCategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                IArticleManager articleManager = new ArticleManager();
                var userid = HttpContext.Session.GetString("userid_session");
                Guid guid = new Guid(userid);
                articleManager.CreateCategory(model.CategoryName, guid);
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError("", "您录入的信息有误");
            return View(model);
        }
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CategoryList()
        {

            var userid = HttpContext.Session.GetString("userid_session");
            Guid guid = new Guid(userid);
            ViewBag.userid = userid.ToString();
            //var userid = Guid.Parse(HttpContext.Session.GetString("userid"));
            return View(await new ArticleManager().GetAllCategories(guid));
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            IArticleManager articleManager = new ArticleManager();
            await articleManager.RemoveCategory(id);
            return RedirectToAction("CategoryList");
        }
        [HttpGet]
        public async Task<ActionResult> EditCategory(Guid id)
        {
            IArticleManager articleManager = new ArticleManager();
            var data = await articleManager.GetOneCategoryById(id);
            return View(new EditCategoryViewModel()
            {
                CategoryId=data.Id,
                CategoryName=data.CategoryName
            });
        }
        [HttpPost]
        public async Task<ActionResult> EditCategory(EditCategoryViewModel model)
        {
            IArticleManager articleManager = new ArticleManager();
            await articleManager.EditCategory(model.CategoryId, model.CategoryName);
            return RedirectToAction("CategoryList");

        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

    }
}
