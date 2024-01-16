using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Identity.Client;
using MvcCoreDemo.Models.Domain;
using MvcCoreDemo.Models.ViewModel;
using System;
using System.Text;

namespace MvcCoreDemo.Helper
{

    //é possibile definire dei helpertag legati a tag pre-esistenti stile decorator con [HtmlTargetElement('<nome-elemento>', Attributes ='<nome-helper>')], 
    //questi saranno aggiunti al tag stesso come se fossero un normale attributo, la classe "output" include le proprietà per manipolare attributi e classi 
    [HtmlTargetElement("blogPost")]
    public class BlogPostTagHelper : TagHelper
    {
        [HtmlAttributeName("post")]
        public BlogPost Post {  get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "post";
            output.TagMode = TagMode.StartTagAndEndTag;

            var sb = new StringBuilder();

            sb.AppendFormat(
                "<div class='mb-5 bg-light box-shadow p-5' style='height: 600px; overflow:auto'>" +
                "<div class='d-flex justify-content-center' style='overflow: hidden;'>");

            sb.AppendFormat(
                $"<img src ='{Post.FeaturedImageUrl}' " +
                $"alt='@blogpost.Heading' " +
                $"class='mb-2 d-block img-fluid' " +
                $"style='width: 100%; height: 200px; object-fit: cover;' />");

            sb.AppendFormat("</div>" +
                $"<h2 class='mb-3'>{Post.Heading}</h2>"+
                " <div class='mb-3 d-flex justify-content-start gap-1'>");

            foreach(var tag in Post.Tags)
            {
                sb.AppendFormat($"<span class='badge bg-secondary pt-1'>{tag.Name}</span>");
            }

            sb.AppendFormat($"</div><p class='mb-3'>{Post.Author} <br />Published Date:{Post.PublishedDate.ToShortDateString()} </p>");
            sb.AppendFormat($"<p class='mb-3'> {Post.ShortDescription} </p>");
            sb.AppendFormat($"<a class='btn btn-dark' href='/userblog?urlhandle={Post.UrlHandle}'>Read More</a>");
            
            sb.AppendFormat($"</div>");

            output.Content.SetHtmlContent(sb.ToString());
         }
    }
}
