using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventWriteDTO, Event>();

            CreateMap<Event, EventReadDTO>()
                .ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.FullName : null))
                .ForMember(dest => dest.RegisteredCount, opt => opt.Ignore())
                .ForMember(dest => dest.SeatsLeft, opt => opt.Ignore());

            CreateMap<Event, EventDetailDTO>()
                .ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.FullName : null))
                .ForMember(dest => dest.RegisteredCount, opt => opt.Ignore())
                .ForMember(dest => dest.SeatsLeft, opt => opt.Ignore())
                .ForMember(dest => dest.TicketClasses, opt => opt.Ignore());
        }
    }
}
