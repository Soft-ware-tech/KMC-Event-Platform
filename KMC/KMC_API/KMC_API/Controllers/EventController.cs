using Microsoft.AspNetCore.Mvc;
using KMC_API.Model;
using KMC_API.Data;
using KMC_API.DTO;
using AutoMapper;

namespace KMC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IMapper mapper;
        private EventRepo repo;
        private TicketClassRepo ticketRepo;

        public EventController(IMapper _mapper, EventRepo _repo, TicketClassRepo _ticketRepo)
        {
            mapper = _mapper;
            repo = _repo;
            ticketRepo = _ticketRepo;
        }

        // GET api/Event  - public listing, used by the "All Events" page.
        [HttpGet]
        public ActionResult<List<EventReadDTO>> GetEvents()
        {
            var events = repo.GetEvents();
            return Ok(events.Select(ToReadDTO).ToList());
        }

        // GET api/Event/5 - full detail, used by the event page + booking form.
        [HttpGet("{id}")]
        public ActionResult<EventDetailDTO> GetEventByID(int id)
        {
            var ev = repo.GetEventByID(id);
            if (ev == null)
                return NotFound();

            var dto = mapper.Map<EventDetailDTO>(ev);
            var registered = repo.CountRegistrations(ev.EventId);
            dto.RegisteredCount = registered;
            dto.SeatsLeft = ev.RegistrationLimit > 0 ? Math.Max(0, ev.RegistrationLimit - registered) : (int?)null;

            dto.TicketClasses = ev.TicketClasses.Select(t =>
            {
                var sold = ticketRepo.CountSold(t.TicketClassId);
                return new TicketClassReadDTO
                {
                    TicketClassId = t.TicketClassId,
                    EventId = t.EventId,
                    ClassName = t.ClassName,
                    Price = t.Price,
                    Capacity = t.Capacity,
                    Sold = sold,
                    Available = Math.Max(0, t.Capacity - sold)
                };
            }).ToList();

            return Ok(dto);
        }

        // GET api/Event/search?keyword=&category=&date=2026-08-20
        [HttpGet("search")]
        public ActionResult<List<EventReadDTO>> Search(string? keyword, string? category, DateTime? date)
        {
            var events = repo.SearchEvents(keyword, category, date);
            return Ok(events.Select(ToReadDTO).ToList());
        }

        // GET api/Event/organizer/5 - "My Events" dashboard for a logged-in organizer.
        [HttpGet("organizer/{organizerId}")]
        public ActionResult<List<EventReadDTO>> GetByOrganizer(int organizerId)
        {
            var events = repo.GetEventsByOrganizer(organizerId);
            return Ok(events.Select(ToReadDTO).ToList());
        }

        // POST api/Event
        [HttpPost]
        public ActionResult AddEvent(EventWriteDTO dto)
        {
            if (dto.EndDate < dto.StartDate)
                return BadRequest("End date cannot be before the start date.");

            var ev = mapper.Map<Event>(dto);
            if (repo.AddEvent(ev))
                return Ok(ToReadDTO(ev));

            return BadRequest();
        }

        // PUT api/Event/5
        // Requirement: "Only the creator should be able to update their events."
        // dto.OrganizerId must match the OrganizerId that owns the existing event.
        [HttpPut("{id}")]
        public ActionResult UpdateEvent(EventWriteDTO dto, int id)
        {
            var existing = repo.GetEventByID(id);
            if (existing == null)
                return NotFound();

            if (existing.OrganizerId != dto.OrganizerId)
                return StatusCode(403, "You are not the owner of this event.");

            if (dto.EndDate < dto.StartDate)
                return BadRequest("End date cannot be before the start date.");

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Category = dto.Category;
            existing.Location = dto.Location;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.ImageUrl = dto.ImageUrl;
            existing.RegistrationLimit = dto.RegistrationLimit;
            existing.HasTickets = dto.HasTickets;

            if (repo.UpdateEvent(existing))
                return Ok();
            return BadRequest();
        }

        // DELETE api/Event/5?organizerId=2
        [HttpDelete("{id}")]
        public ActionResult DeleteEvent(int id, int organizerId)
        {
            var existing = repo.GetEventByID(id);
            if (existing == null)
                return NotFound();

            if (existing.OrganizerId != organizerId)
                return StatusCode(403, "You are not the owner of this event.");

            if (repo.RemoveEvent(existing))
                return Ok();
            return BadRequest();
        }

        private EventReadDTO ToReadDTO(Event ev)
        {
            var dto = mapper.Map<EventReadDTO>(ev);
            var registered = repo.CountRegistrations(ev.EventId);
            dto.RegisteredCount = registered;
            dto.SeatsLeft = ev.RegistrationLimit > 0 ? Math.Max(0, ev.RegistrationLimit - registered) : (int?)null;
            return dto;
        }
    }
}
