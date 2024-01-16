using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcCoreDemo.Data.Interfacce;

namespace MvcCoreDemo.Controllers
{
    [Authorize]
    public class UserBlogController(IBlogPostDal blogDal) : Controller
    {
        private readonly IBlogPostDal _blogDal = blogDal;

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var post = await _blogDal.GetByUrlHandleAsync(urlHandle);
            
            return View(post);
        }
    }
}
