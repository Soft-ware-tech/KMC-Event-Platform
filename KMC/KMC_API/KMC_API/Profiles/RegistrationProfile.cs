using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {
            CreateMap<RegistrationWriteDTO, Registration>();

            CreateMap<Registration, RegistrationReadDTO>()
                .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.ForEvent != null ? src.ForEvent.Title : null))
                .ForMember(dest => dest.TicketClassName, opt => opt.MapFrom(src => src.BookedClass != null ? src.BookedClass.ClassName : null));
        }
    }
}
