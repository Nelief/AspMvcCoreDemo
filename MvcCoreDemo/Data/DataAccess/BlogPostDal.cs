using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using MvcCoreDemo.Data;
using MvcCoreDemo.Data.Interfacce;
using MvcCoreDemo.Models.Domain;
using System.Runtime.InteropServices;

namespace MvcCoreDemo.Data.DataAccess
{
    public class BlogPostDal(BlogDbContext context) : IBlogPostDal
    {
        public readonly BlogDbContext _context = context;

        public async Task<BlogPost> AddAsync(BlogPost blogpost)
        {
            await _context.Posts.AddAsync(blogpost);
            await _context.SaveChangesAsync();
            return blogpost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var postList = await _context.Posts.Include(x => x.Tags).ToListAsync();
            return postList;
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            var postList = await _context.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
            return postList;
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogpost)
        {
            var oldPost = await _context.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogpost.Id);

            if (oldPost != null)
            {
                oldPost.Author = blogpost.Author;
                oldPost.ShortDescription = blogpost.ShortDescription;
                oldPost.PublishedDate = blogpost.PublishedDate;
                oldPost.Visible = blogpost.Visible;
                oldPost.PageTitle = blogpost.PageTitle;
                oldPost.Content = blogpost.Content;
                oldPost.FeaturedImageUrl = blogpost.FeaturedImageUrl;
                oldPost.Heading = blogpost.Heading;
                oldPost.UrlHandle = blogpost.UrlHandle;
                oldPost.Tags = blogpost.Tags;

                await _context.SaveChangesAsync();
                return oldPost;
            }
            return null;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var postToDelete = await _context.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id ==id);
            if (postToDelete != null)
            {
                _context.Posts.Remove(postToDelete);
                await _context.SaveChangesAsync();
                return postToDelete;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _context.Posts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.UrlHandle ==urlHandle);
        }
    }
}
