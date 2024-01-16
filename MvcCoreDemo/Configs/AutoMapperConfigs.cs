using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MvcCoreDemo.Models.Domain;
using MvcCoreDemo.Models.ViewModel;

namespace MvcCoreDemo.Configs
{
    public class AutoMapperConfigs :Profile
    {
        public AutoMapperConfigs()
        {
            CreateMap<AddTagDTO, Tag>();
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();

            CreateMap<BlogPost,BlogPostDTO>();
            CreateMap<AddPostDTO, BlogPost>();

            CreateMap<IdentityUser, UserDTO>()
                .ForMember(x => x.Id, o => o.MapFrom(x => Guid.Parse(x.Id)))
                .ForMember(x => x.Username, o => o.MapFrom(x => x.UserName))
                .ForMember(x => x.Email, o => o.MapFrom(x => x.Email));
        }
    }
}
