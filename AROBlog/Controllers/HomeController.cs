using AROBlog.BLL;
using AROBlog.Filter;
using AROBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AROBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task< IActionResult> Index()
        {
            ViewBag.CategoryIds = await new ArticleManager().GetAllCategories();
            var articleMgr = new ArticleManager();
            var articles = await articleMgr.GetAllArticles();
            return View(articles);
        }
        [AROBlogAuth]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}