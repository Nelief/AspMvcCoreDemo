using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcCoreDemo.Data.Interfacce;

namespace MvcCoreDemo.Data.DataAccess
{
    public class UserDal(AuthDbContext context) : IUserDal
    {
        private readonly AuthDbContext context = context;

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var UserList = await context.Users.ToListAsync();
            var superAdmin = await context.Users.FirstOrDefaultAsync(x => x.Email == "superAdmin@gmail.com");
            if (superAdmin != null)
            {
                //questo rimuove il super admin SOLO dalla lista locale, per rimuoverlo dal DB sarebbe necessario chiamare "savechanges" 
                UserList.Remove(superAdmin);
            }

            return UserList;
        }

        public  IEnumerable<IdentityRole> GetRolesById(Guid id)
        {
            var UserRoleList = context.UserRoles.Where(x => x.UserId == id.ToString()).ToList();

            var RoleList = new List<IdentityRole>();

            foreach (var userRole in UserRoleList)
            {
                var role = context.Roles.Find(userRole.RoleId);
                if(role != null)
                {
                    RoleList.Add(role);
                }
            }

            return RoleList;
            
        }
    }
}
