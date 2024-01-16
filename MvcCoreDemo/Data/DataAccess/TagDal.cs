using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using MvcCoreDemo.Data;
using MvcCoreDemo.Data.Interfacce;
using MvcCoreDemo.Models.Domain;
using System;

namespace MvcCoreDemo.Data.DataAccess
{
    public class TagDal(BlogDbContext context) : ITagDal
    {
        public readonly BlogDbContext _context = context;
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return tag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var tagInDb = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);

            if (tagInDb != null)
            {
                tagInDb.Name = tag.Name;
                tagInDb.DisplayName = tag.DisplayName;
                await _context.SaveChangesAsync();
                return tagInDb;
            }

            return null;
        }
    }
}
