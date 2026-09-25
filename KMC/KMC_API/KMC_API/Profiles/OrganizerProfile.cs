using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class OrganizerProfile : Profile
    {
        public OrganizerProfile()
        {
            CreateMap<OrganizerRegisterDTO, Organizer>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<Organizer, OrganizerReadDTO>();
        }
    }
}
