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
            //CreateMap<NewsArticle, NewsArticleDTO>().ReverseMap();

            CreateMap<NewsArticle, NewsArticleDTO>()
            .ForMember(dest => dest.SelectedTagIds, opt =>
                opt.MapFrom(src => src.Tags.Select(t => t.TagId).ToList()));

            CreateMap<NewsArticleDTO, NewsArticle>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<SystemAccount, SystemAccountDTO>().ReverseMap();
            CreateMap<Tag, TagDTO>().ReverseMap();
        }
    }
}
