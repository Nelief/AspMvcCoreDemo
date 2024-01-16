
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MvcCoreDemo.Data.Interfacce;
using System.Net;

namespace MvcCoreDemo.Data.DataAccess
{
    public class CouldImageDal : IImgDal
    {

        public readonly IConfiguration configuration;
        private readonly Account account;

        public CouldImageDal(IConfiguration config)
        {
            configuration = config;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
                );
        }

    

        public async Task<string?> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }

        Task<byte[]> IImgDal.GetImage(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
