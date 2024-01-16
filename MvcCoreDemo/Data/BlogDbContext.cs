using Microsoft.EntityFrameworkCore;
using MvcCoreDemo.Models.Domain;

namespace MvcCoreDemo.Data
{
    public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
    {
        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
