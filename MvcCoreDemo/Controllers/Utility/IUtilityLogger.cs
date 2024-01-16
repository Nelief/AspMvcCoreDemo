using Microsoft.AspNetCore.Identity;

namespace MvcCoreDemo.Controllers.Utility
{
    public interface IUtilityLogger
    {
        public void LogInfoUserDetails(string message);
        public void LogInfoNoUser(string message);
    }
}
