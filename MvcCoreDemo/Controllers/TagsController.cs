using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCoreDemo.Data;
using MvcCoreDemo.Data.Interfacce;
using MvcCoreDemo.Models.Domain;
using MvcCoreDemo.Models.ViewModel;

namespace MvcCoreDemo.Controllers
{

    enum TagViewNames
    {
        Add,
        List,
        Edit
    }

    [Authorize (Roles = "ADMIN")]
    public class TagsController(ITagDal dal, IMapper mapper) : Controller
    {
        public readonly IMapper _mapper = mapper;
        public readonly ITagDal _dal = dal;

        // /Tags/List
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            IEnumerable<Tag> TagList = await _dal.GetAllAsync();

            List<TagDTO> model = TagList.Select(_mapper.Map<Tag, TagDTO>).ToList();

            return View(TagViewNames.List.ToString(), model);
        }

        // /Tags/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View(TagViewNames.Add.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddTagDTO model)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<AddTagDTO, Tag>(model);

                await _dal.AddAsync(tag);

                return RedirectToAction(TagViewNames.List.ToString());
            }
            return View(TagViewNames.Add.ToString(), model);
        }

        // /Tags/Edit/x
        [HttpGet]
        public async Task<IActionResult> EditAsync(Guid? Id)
        {
            if (!Id.HasValue || Id.Equals(0)) return BadRequest(ModelState);

            var tag = await _dal.GetAsync(Id.Value);

            if (tag == null) return NotFound();

            return View(TagViewNames.Edit.ToString(), _mapper.Map<Tag, TagDTO>(tag));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(TagDTO model)
        {
            if (ModelState.IsValid)
            {
                Tag tag = _mapper.Map<TagDTO, Tag>(model);

                var updatedTag = await _dal.UpdateAsync(tag);

                if (updatedTag == null) return NotFound();

                return RedirectToAction(TagViewNames.List.ToString());
            }
            return View(TagViewNames.Edit.ToString(), model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(Guid? Id)
        {
            if (Id.Equals(0) || Id == null) return BadRequest(ModelState);

            var deletedTag = await _dal.DeleteAsync(Id.Value);

            if (deletedTag == null) return NotFound();

            return RedirectToAction(TagViewNames.List.ToString());
        }
    }
}

////Form Handling tramite request parsing 
////questo approccio crea un problema con il nome della action, non avendo parametri in ingresso la action non può chiamarsi come quella per prendere il form (crea un conflitto, sarà quindi necessario cambiare nome. 
///i nomi usati nella chiamata .Form vanno specificati sulal view con il tag "name" 
//[HttpPost]
//public IActionResult Save()
//{
//    var name = Request.Form["name"];
//    var displayName = Request.Form["displayName"];
//    return View("Add");
//}  