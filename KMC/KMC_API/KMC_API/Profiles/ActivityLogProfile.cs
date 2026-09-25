using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class ActivityLogProfile : Profile
    {
        public ActivityLogProfile()
        {
            CreateMap<ActivityLog, ActivityLogReadDTO>();
        }
    }
}