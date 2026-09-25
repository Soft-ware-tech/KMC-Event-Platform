using Microsoft.AspNetCore.Mvc;
using KMC_API.Model;
using KMC_API.Data;
using KMC_API.DTO;
using AutoMapper;

namespace KMC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketClassController : ControllerBase
    {
        private IMapper mapper;
        private TicketClassRepo repo;
        private EventRepo eventRepo;

        public TicketClassController(IMapper _mapper, TicketClassRepo _repo, EventRepo _eventRepo)
        {
            mapper = _mapper;
            repo = _repo;
            eventRepo = _eventRepo;
        }

        // GET api/TicketClass/event/5 - the ticket tiers offered by one event.
        [HttpGet("event/{eventId}")]
        public ActionResult<List<TicketClassReadDTO>> GetByEvent(int eventId)
        {
            var classes = repo.GetTicketClassesByEvent(eventId);
            return Ok(classes.Select(ToReadDTO).ToList());
        }

        // POST api/TicketClass
        [HttpPost]
        public ActionResult AddTicketClass(TicketClassWriteDTO dto)
        {
            var ev = eventRepo.GetEventByID(dto.EventId);
            if (ev == null)
                return NotFound("Event not found.");

            if (ev.OrganizerId != dto.OrganizerId)
                return StatusCode(403, "You are not the owner of this event.");

            if (!ev.HasTickets)
                return BadRequest("This event is not configured to sell tickets.");

            var ticketClass = mapper.Map<TicketClass>(dto);
            if (repo.AddTicketClass(ticketClass))
                return Ok(ToReadDTO(ticketClass));

            return BadRequest();
        }

        // PUT api/TicketClass/5
        [HttpPut("{id}")]
        public ActionResult UpdateTicketClass(TicketClassWriteDTO dto, int id)
        {
            var existing = repo.GetTicketClassByID(id);
            if (existing == null)
                return NotFound();

            var ev = eventRepo.GetEventByID(existing.EventId);
            if (ev == null || ev.OrganizerId != dto.OrganizerId)
                return Forbid();

            existing.ClassName = dto.ClassName;
            existing.Price = dto.Price;
            existing.Capacity = dto.Capacity;

            if (repo.UpdateTicketClass(existing))
                return Ok();
            return BadRequest();
        }

        // DELETE api/TicketClass/5?organizerId=2
        [HttpDelete("{id}")]
        public ActionResult DeleteTicketClass(int id, int organizerId)
        {
            var existing = repo.GetTicketClassByID(id);
            if (existing == null)
                return NotFound();

            var ev = eventRepo.GetEventByID(existing.EventId);
            if (ev == null || ev.OrganizerId != organizerId)
                return Forbid();

            if (repo.RemoveTicketClass(existing))
                return Ok();
            return BadRequest();
        }

        private TicketClassReadDTO ToReadDTO(TicketClass t)
        {
            var sold = repo.CountSold(t.TicketClassId);
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
        }
    }
}
