using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MvcCoreDemo.Controllers.Utility
{
    public class UserLogger(IHttpContextAccessor httpContext, ILogger<AccountController> logger) : IUtilityLogger
    {
        private readonly IHttpContextAccessor httpContext = httpContext;
        private readonly ILogger<AccountController> logger = logger;

        public void LogInfoNoUser(string message)
        {
            logger.LogInformation(message);
        }

        public void LogInfoUserDetails(string message)
        {
            var id = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var name = httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            logger.LogInformation($"User {id}|{name} : {message}");
        }
    }
}
