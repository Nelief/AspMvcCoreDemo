using MvcCoreDemo.Models.Domain;

namespace MvcCoreDemo.Models.ViewModel
{
    public class HomeDTO
    {
        public List<BlogPost> Blogs { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
