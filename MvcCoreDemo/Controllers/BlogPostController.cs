using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcCoreDemo.Data.Interfacce;
using MvcCoreDemo.Models.Domain;
using MvcCoreDemo.Models.ViewModel;
using System.Diagnostics;

namespace MvcCoreDemo.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class BlogPostController(IBlogPostDal dal, ITagDal tagDal, IMapper mapper) : Controller
    {
        public readonly IBlogPostDal _dal = dal;
        public readonly ITagDal _tagDal = tagDal;
        public readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var blogList = await _dal.GetAllAsync();
            var blogListDTO = blogList.Select(_mapper.Map<BlogPost, BlogPostDTO>).ToList();
            return View(blogListDTO);
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            var tagList = await _tagDal.GetAllAsync();

            //creazione DTO con elemento SelectListItem, questo è gia formattato con Text/Value per essere facilmente renderizzato sulla view 
            var model = new AddPostDTO()
            {
                Tags = tagList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddPostDTO dto)
        {
            var post = _mapper.Map<AddPostDTO, BlogPost>(dto);

            var taglist = new List<Tag>();
            foreach (var tagId in dto.SelectedTags)
            {
                var tag = await _tagDal.GetAsync(Guid.Parse(tagId));
                if (tag != null)
                {
                    taglist.Add(tag);
                }
            }

            post.Tags = taglist;

            await _dal.AddAsync(post);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            var post = await _dal.GetAsync(id);
            var taglist = await _tagDal.GetAllAsync();

            if(post != null)
            {
                //costruizione del DTO a partire da Model e Lista Tag, "tags" include TUTTE le tag esistenti in formato selezionabile mentre Selected tags conterrà un Array di stringhe con gli ID gia selezionati. 
                var model = new EditPostDTO()
                {
                    Id = post.Id,
                    PageTitle = post.PageTitle,
                    Heading = post.Heading,
                    ShortDescription = post.ShortDescription,
                    Content = post.Content,
                    Author = post.Author,
                    UrlHandle = post.UrlHandle,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    PublishedDate = post.PublishedDate,
                    Visible = post.Visible,
                    Tags = taglist.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = post.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostDTO dto)
        {
            //mapping iniziale
            var model = new BlogPost()
            {
                Id = dto.Id,
                Heading = dto.Heading,
                PageTitle = dto.PageTitle,
                Content = dto.Content,
                Author = dto.Author,
                ShortDescription = dto.ShortDescription,
                FeaturedImageUrl = dto.FeaturedImageUrl,
                PublishedDate = dto.PublishedDate,
                UrlHandle = dto.UrlHandle,
                Visible = dto.Visible
            };

            //tag list mapping 
            var selectedTags = new List<Tag>();
            
            foreach(var selectedTagId in dto.SelectedTags)
            {
                if(Guid.TryParse(selectedTagId, out var tagId))
                {
                    var tag = await _tagDal.GetAsync(tagId);
                    if(tag != null)
                    {
                        selectedTags.Add(tag);
                    }
                }
            }
            model.Tags = selectedTags;

            //chiamata al dal e verifica update 
            var updatedBlog = await _dal.UpdateAsync(model);

            if (updatedBlog != null)
            {
                return RedirectToAction("List");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedPost =  await _dal.DeleteAsync(id);
            if(deletedPost != null)
            {
                return RedirectToAction("List");
            }
            return NotFound();
        }
    }
}
