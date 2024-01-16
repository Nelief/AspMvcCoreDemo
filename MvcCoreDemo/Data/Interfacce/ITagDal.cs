using MvcCoreDemo.Models.Domain;

namespace MvcCoreDemo.Data.Interfacce
{
    public interface ITagDal
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag); //nullable dato che potrebbe non esistere nel DB 

        Task<Tag?> DeleteAsync(Guid id); //nullable dato che potrebbe non esistere nel DB 
    }
}
