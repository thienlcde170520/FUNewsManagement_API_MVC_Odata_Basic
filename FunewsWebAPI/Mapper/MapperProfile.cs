using AutoMapper;
using BusinessObjects.Models;
using FUnewsDTO;

namespace FunewsWebAPI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ParentCategoryName, opt =>
                    opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : "None"));
            CreateMap<NewsArticle, NewsArticleDTO>().ReverseMap();
            CreateMap<SystemAccount, SystemAccountDTO>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();
        }
    }
}
