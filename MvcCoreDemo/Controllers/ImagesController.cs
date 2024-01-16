using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreDemo.Data.Interfacce;
using System.Net;

namespace MvcCoreDemo.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController(IImgDal imgDal) : ControllerBase
    {
        private readonly IImgDal imgDal = imgDal;

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("test string");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imgUrl = await imgDal.UploadAsync(file);
            if (imgUrl == null) return Problem("Image Upload Failed",null,(int)HttpStatusCode.InternalServerError);
            return new JsonResult(new {link = imgUrl});
        }

        [HttpGet]
        [Route("{filename}")]
        [Authorize]
        public async Task<IActionResult> getimage(string filename)
        {
            var filecontent = await imgDal.GetImage(filename);

            if (filecontent != null)
            {
                var contenttype = GetContentType(filename);

                return File(filecontent, contenttype);
            }

            return NotFound();
        }

        private static string GetContentType(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
