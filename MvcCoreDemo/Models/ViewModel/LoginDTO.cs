using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcCoreDemo.Models.ViewModel
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string? ReturnUrl { get; set; }  
    }
}
