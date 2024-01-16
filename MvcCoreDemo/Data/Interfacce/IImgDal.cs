namespace MvcCoreDemo.Data.Interfacce
{
    public interface IImgDal
    {
        Task<string?> UploadAsync(IFormFile file);
        Task<byte[]> GetImage(string filename);

    }
}
