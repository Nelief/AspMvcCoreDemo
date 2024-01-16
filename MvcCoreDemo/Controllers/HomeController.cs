using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcCoreDemo.Data.Interfacce;
using MvcCoreDemo.Models;
using MvcCoreDemo.Models.ViewModel;
using System.Diagnostics;

namespace MvcCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly IBlogPostDal _blogDal;
        private readonly ITagDal _tagDal;

        public HomeController(ILogger<HomeController> logger,IBlogPostDal blogDal,ITagDal tagDal)
        {
            _logger = logger;
            _blogDal = blogDal;
            _tagDal = tagDal;
        }

        public async Task<IActionResult> Index()
        {
            var blogList = await _blogDal.GetAllAsync();

            var tagList = await _tagDal.GetAllAsync();

            var dto = new HomeDTO()
            {
                Blogs = blogList.ToList(),
                Tags = tagList.ToList()
            };

            return View(dto);
        }

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