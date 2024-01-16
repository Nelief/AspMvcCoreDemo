using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace MvcCoreDemo.Models.ViewModel
{
    public class EditUserDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email {  get; set; }
        public bool IsAdmin { get; set; }

    }
}
