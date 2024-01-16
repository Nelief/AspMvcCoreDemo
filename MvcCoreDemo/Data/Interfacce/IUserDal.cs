using Microsoft.AspNetCore.Identity;

namespace MvcCoreDemo.Data.Interfacce
{
    public interface IUserDal
    {
        Task<IEnumerable<IdentityUser>> GetAll();
        IEnumerable<IdentityRole> GetRolesById(Guid id);
    }
}
