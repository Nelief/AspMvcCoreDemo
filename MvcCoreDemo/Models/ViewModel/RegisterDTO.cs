using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcCoreDemo.Models.ViewModel
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [PasswordPropertyText]
        [MinLength(6,ErrorMessage = "Password has to be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
