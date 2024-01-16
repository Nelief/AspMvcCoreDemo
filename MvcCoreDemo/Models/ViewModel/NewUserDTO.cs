using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcCoreDemo.Models.ViewModel
{
    public class NewUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool AdminRoleChek {  get; set; }

    }
}
