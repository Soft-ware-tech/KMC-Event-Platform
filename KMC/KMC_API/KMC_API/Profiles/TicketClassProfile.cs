using AutoMapper;
using KMC_API.Model;
using KMC_API.DTO;

namespace KMC_API.Profiles
{
    public class TicketClassProfile : Profile
    {
        public TicketClassProfile()
        {
            CreateMap<TicketClassWriteDTO, TicketClass>();

            CreateMap<TicketClass, TicketClassReadDTO>()
                .ForMember(dest => dest.Sold, opt => opt.Ignore())
                .ForMember(dest => dest.Available, opt => opt.Ignore());
        }
    }
}
