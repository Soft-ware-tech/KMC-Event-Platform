using Microsoft.AspNetCore.Mvc;
using KMC_API.Model;
using KMC_API.Data;
using KMC_API.DTO;
using AutoMapper;

namespace KMC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IMapper mapper;
        private RegistrationRepo repo;
        private EventRepo eventRepo;
        private TicketClassRepo ticketRepo;

        public RegistrationController(IMapper _mapper, RegistrationRepo _repo, EventRepo _eventRepo, TicketClassRepo _ticketRepo)
        {
            mapper = _mapper;
            repo = _repo;
            eventRepo = _eventRepo;
            ticketRepo = _ticketRepo;
        }

        // POST api/Registration
        [HttpPost]
        public ActionResult<RegistrationReadDTO> Register(RegistrationWriteDTO dto)
        {
            if (dto.Quantity < 1)
                return BadRequest("Quantity must be at least 1.");

            var ev = eventRepo.GetEventByID(dto.EventId);
            if (ev == null)
                return NotFound("Event not found.");

            if (DateTime.Now.Date > ev.EndDate.Date)
                return BadRequest("Registration is closed - this event has already ended.");

            if (ev.RegistrationLimit > 0)
            {
                var alreadyBooked = repo.TotalBookedForEvent(ev.EventId);
                if (alreadyBooked + dto.Quantity > ev.RegistrationLimit)
                    return BadRequest("Sorry, this event is fully booked.");
            }

            var registration = mapper.Map<Registration>(dto);
            registration.PaymentStatus = "N/A";
            registration.AmountPaid = 0;

            if (ev.HasTickets)
            {
                if (dto.TicketClassId == null)
                    return BadRequest("Please select a ticket class (1st / 2nd / 3rd Class).");

                var ticketClass = ticketRepo.GetTicketClassByID(dto.TicketClassId.Value);
                if (ticketClass == null || ticketClass.EventId != ev.EventId)
                    return BadRequest("Invalid ticket class for this event.");

                var soldForClass = repo.TotalBookedForTicketClass(ticketClass.TicketClassId);
                if (soldForClass + dto.Quantity > ticketClass.Capacity)
                    return BadRequest($"Sorry, {ticketClass.ClassName} tickets are sold out.");

                var payment = ProcessMockPayment(dto, ticketClass.Price * dto.Quantity);
                if (!payment.Approved)
                    return BadRequest(payment.Message);

                registration.TicketClassId = ticketClass.TicketClassId;
                registration.CardHolderName = dto.CardHolderName;
                registration.CardLast4 = payment.CardLast4;
                registration.AmountPaid = ticketClass.Price * dto.Quantity;
                registration.PaymentStatus = "Paid";
            }
            else
            {
                registration.TicketClassId = null;
            }

            if (repo.AddRegistration(registration))
            {
                var saved = repo.GetRegistrationByID(registration.RegistrationId);
                return Ok(mapper.Map<RegistrationReadDTO>(saved));
            }

            return BadRequest();
        }

        // GET api/Registration/event/5 - attendee list, used on the organizer dashboard.
        [HttpGet("event/{eventId}")]
        public ActionResult<List<RegistrationReadDTO>> GetByEvent(int eventId)
        {
            var regs = repo.GetRegistrationsByEvent(eventId);
            return Ok(mapper.Map<List<RegistrationReadDTO>>(regs));
        }

        // GET api/Registration - EVERY booking across every event (Manager use only)
        [HttpGet]
        public ActionResult<List<RegistrationReadDTO>> GetAllRegistrations()
        {
            var regs = repo.GetAllRegistrations();
            return Ok(mapper.Map<List<RegistrationReadDTO>>(regs));
        }

        // GET api/Registration/5 - used as an e-ticket / booking confirmation.
        [HttpGet("{id}")]
        public ActionResult<RegistrationReadDTO> GetByID(int id)
        {
            var reg = repo.GetRegistrationByID(id);
            if (reg == null)
                return NotFound();
            return Ok(mapper.Map<RegistrationReadDTO>(reg));
        }

        private (bool Approved, string Message, string CardLast4) ProcessMockPayment(RegistrationWriteDTO dto, decimal amount)
        {
            var cardNumber = (dto.CardNumber ?? string.Empty).Replace(" ", "");

            if (string.IsNullOrWhiteSpace(dto.CardHolderName))
                return (false, "Card holder name is required.", "");

            if (cardNumber.Length < 12 || cardNumber.Length > 19 || !cardNumber.All(char.IsDigit))
                return (false, "Card number is invalid.", "");

            if (string.IsNullOrWhiteSpace(dto.Cvv) || dto.Cvv.Length < 3 || dto.Cvv.Length > 4)
                return (false, "CVV is invalid.", "");

            if (string.IsNullOrWhiteSpace(dto.ExpiryMonth) || string.IsNullOrWhiteSpace(dto.ExpiryYear))
                return (false, "Card expiry date is required.", "");

            var last4 = cardNumber.Substring(cardNumber.Length - 4);
            return (true, "Approved", last4);
        }
    }
}