using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcCoreDemo.Models.ViewModel
{
    public class EditPostDTO
    {
        public Guid Id { get; set; }

        public string Heading { get; set; }

        public string PageTitle { get; set; }

        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }

        public bool Visible { get; set; }

        //tag picker 
        public IEnumerable<SelectListItem> Tags { get; set; }

        public string[] SelectedTags { get; set; } = [];
    }
}
