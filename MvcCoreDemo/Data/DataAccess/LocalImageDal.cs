
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing.Constraints;
using MvcCoreDemo.Data.Interfacce;

namespace MvcCoreDemo.Data.DataAccess
{
    public class LocalImageDal(IWebHostEnvironment env) : IImgDal
    {
        //classe iniettata con la quale ottenere la rootPath
        private readonly IWebHostEnvironment env = env;

        public async Task<string?> UploadAsync(IFormFile file)
        {
            //path per l' upload sul server 
            var filePath = Path.Combine(env.WebRootPath,"images", file.FileName);

            using var filestream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(filestream);

            //path per la URL dell' endpoint da salvare sull model 
            var apiImageCall = Path.Combine("\\api\\Images", file.FileName);

            return apiImageCall;
        }

        public async Task<byte[]> GetImage(string filename)
        {
            var imagePath = Path.Combine(env.WebRootPath, "images", filename);

            if (File.Exists(imagePath))
            {
                return await File.ReadAllBytesAsync(imagePath);
            }

            return null;
        }    
    }
}
