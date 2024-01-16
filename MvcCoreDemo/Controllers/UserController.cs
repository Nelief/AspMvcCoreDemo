using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using MvcCoreDemo.Controllers.Utility;
using MvcCoreDemo.Data;
using MvcCoreDemo.Data.Interfacce;
using MvcCoreDemo.Models.ViewModel;

namespace MvcCoreDemo.Controllers
{
    public class UserController(IUserDal dal, IMapper mapper, UserManager<IdentityUser> manager,IUtilityLogger logger) : Controller
    {
        private readonly IUserDal dal = dal;
        private readonly IMapper mapper = mapper;
        private readonly UserManager<IdentityUser> manager = manager;
        private readonly IUtilityLogger logger = logger;

        // GET /User/List
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> List()
        {
            //fetch utenti 
            var UserList = (await dal.GetAll()).Select(mapper.Map<IdentityUser, UserDTO>).ToList();

            //fetch ruoli x ogni utente
            foreach (var User in UserList)
            {
                User.Roles = dal.GetRolesById(User.Id).Select(x => x.Name).ToList();
            }

            //creazione DTO
            var UserDto = new UserManagerDTO()
            {
                Users = UserList
            };

            return View(UserDto);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> List(NewUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = dto.UserName,
                    Email = dto.Email
                };

                var result = await manager.CreateAsync(user, dto.Password);

                if (result is not null && result.Succeeded)
                {
                    var roles = new List<string>() { ROLES.USER.ToString() };
                    if (dto.AdminRoleChek)
                    {
                        roles.Add(ROLES.ADMIN.ToString());
                    }

                    result = await manager.AddToRolesAsync(user, roles);

                    if (result is not null && result.Succeeded)
                    {
                        logger.LogInfoUserDetails($"Created new user : {user.Id}|{user.UserName}");
                    }
                }

            }
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await manager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var result = await manager.DeleteAsync(user);

                if(result != null && result.Succeeded)
                {
                    logger.LogInfoUserDetails($"Deleted the user : {user.Id}|{user.UserName}");
                    return RedirectToAction("List", "User");
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await manager.FindByIdAsync(id.ToString());

            if(user is not null)
            {
                var roles = await manager.GetRolesAsync(user);

                bool isAdmin = false;
                if (roles.Any(x => x.Equals("ADMIN"))) isAdmin = true;

                var dto = new EditUserDTO()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    IsAdmin = isAdmin
                };
                return View(dto);
            }
            return RedirectToAction("List","User");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(EditUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            
            var user = await manager.FindByIdAsync(dto.Id);

            if(user is not null)
            {
                await manager.SetUserNameAsync(user,dto.UserName);
                await manager.SetEmailAsync(user, dto.Email);
                if(dto.IsAdmin)
                {
                    await manager.AddToRoleAsync(user, ROLES.ADMIN.ToString());
                }
                else
                {
                    await manager.RemoveFromRoleAsync(user,ROLES.ADMIN.ToString());   
                }

                logger.LogInfoUserDetails($"Edited the user details of user : {user.Id}|{user.UserName}");
            }
            return RedirectToAction("List");
        }
    }
}
