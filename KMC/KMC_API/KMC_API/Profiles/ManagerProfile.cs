using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class ManagerProfile : Profile
    {
        public ManagerProfile()
        {
            CreateMap<Manager, ManagerReadDTO>();
        }
    }
}