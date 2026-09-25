using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryReadDTO>();
            CreateMap<CategoryCreateDTO, Category>();
        }
    }
}