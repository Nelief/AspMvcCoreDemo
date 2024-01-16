using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MvcCoreDemo.Controllers.Utility;
using MvcCoreDemo.Data;
using MvcCoreDemo.Models.ViewModel;
using System.Security.Claims;

namespace MvcCoreDemo.Controllers
{
    public class AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,IUtilityLogger logger) : Controller
    {
        private readonly UserManager<IdentityUser> userManager = userManager;
        private readonly SignInManager<IdentityUser> signInManager = signInManager;
        private readonly IUtilityLogger logger = logger;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var identityUser = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            //passare la password direttamente al userManager.Create genera la hash implicitamente
            var identityResult =  await userManager.CreateAsync(identityUser,dto.Password);
            

            if(identityResult.Succeeded)
            {
                //Assegnamento del ruolo
               var roleIdentityResult =  await userManager.AddToRoleAsync(identityUser,ROLES.USER.ToString());
               if(roleIdentityResult.Succeeded)
                {
                    logger.LogInfoNoUser($"New User {identityUser.Id}|{identityUser.UserName} : REGISTER");

                    //chiamata al metodo login creando un loginDTO per accedere con l' account appena creato
                    return await Login(new LoginDTO()
                    {
                        Username = dto.Username,
                        Password = dto.Password
                    });
                }
            }

            return RedirectToAction("Index","Home");
        }

        [HttpGet] 
        public IActionResult Login(string ReturnUrl)
        {
            var LoginDto = new LoginDTO()
            {
                ReturnUrl = ReturnUrl
            };
            return View(LoginDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            //validation check eseguito senza modelstate, in questo caso è necessario perchè il model mantiene l' informazione "ReturnURl" che può essere null senza invalidare il model
            if(dto.Username != null && dto.Password != null)
            {
                var signInResult = await signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

                if (signInResult == null || !signInResult.Succeeded)
                {
                    return View(dto);
                }


                logger.LogInfoUserDetails("LOGIN");

                if (!string.IsNullOrWhiteSpace(dto.ReturnUrl))
                {
                    return Redirect(dto.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(dto);  
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            logger.LogInfoUserDetails("LOGOUT");

            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
