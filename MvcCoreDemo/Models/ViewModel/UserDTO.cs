namespace MvcCoreDemo.Models.ViewModel
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public List<string?> Roles { get; set; }
    }
}
