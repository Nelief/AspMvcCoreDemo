using MvcCoreDemo.Models.Domain;

namespace MvcCoreDemo.Data.Interfacce
{
    public interface IBlogPostDal
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> AddAsync(BlogPost blogpost);

        Task<BlogPost?> UpdateAsync(BlogPost blogpost);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
