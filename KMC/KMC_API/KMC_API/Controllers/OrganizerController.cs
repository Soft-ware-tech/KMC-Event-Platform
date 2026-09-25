using Microsoft.AspNetCore.Mvc;
using KMC_API.Model;
using KMC_API.Data;
using KMC_API.DTO;
using KMC_API.Helpers;
using AutoMapper;

namespace KMC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizerController : ControllerBase
    {
        private IMapper mapper;
        private OrganizerRepo repo;
        private EventRepo eventRepo;
        private ActivityLogRepo activityRepo;

        public OrganizerController(IMapper _mapper, OrganizerRepo _repo, EventRepo _eventRepo, ActivityLogRepo _activityRepo)
        {
            mapper = _mapper;
            repo = _repo;
            eventRepo = _eventRepo;
            activityRepo = _activityRepo;
        }

        // POST api/Organizer/register
        [HttpPost("register")]
        public ActionResult<OrganizerReadDTO> Register(OrganizerRegisterDTO dto)
        {
            if (repo.UsernameExists(dto.Username))
                return Conflict("Username is already taken.");

            if (repo.EmailExists(dto.Email))
                return Conflict("An account with this email already exists.");

            var organizer = mapper.Map<Organizer>(dto);
            organizer.PasswordHash = PasswordHelper.Hash(dto.Password);

            if (repo.AddOrganizer(organizer))
                return Ok(mapper.Map<OrganizerReadDTO>(organizer));

            return BadRequest();
        }

        // POST api/Organizer/login
        [HttpPost("login")]
        public ActionResult<OrganizerReadDTO> Login(OrganizerLoginDTO dto)
        {
            var organizer = repo.GetOrganizerByUsername(dto.Username);
            if (organizer == null || !PasswordHelper.Verify(dto.Password, organizer.PasswordHash))
                return Unauthorized("Invalid username or password.");

            if (!organizer.IsActive)
                return Unauthorized("Your account has been suspended by the platform.");

            return Ok(mapper.Map<OrganizerReadDTO>(organizer));
        }

        // GET api/Organizer/5
        [HttpGet("{id}")]
        public ActionResult<OrganizerReadDTO> GetByID(int id)
        {
            var organizer = repo.GetOrganizerByID(id);
            if (organizer != null)
                return Ok(mapper.Map<OrganizerReadDTO>(organizer));
            return NotFound();
        }

        // GET api/Organizer - list every organizer (used by the Manager dashboard)
        [HttpGet]
        public ActionResult<List<OrganizerReadDTO>> GetAllOrganizers()
        {
            var organizers = repo.GetAllOrganizers();
            return Ok(mapper.Map<List<OrganizerReadDTO>>(organizers));
        }

        // DELETE api/Organizer/5?managerId=1
        [HttpDelete("{id}")]
        public ActionResult DeleteOrganizer(int id, int managerId)
        {
            var organizer = repo.GetOrganizerByID(id);
            if (organizer == null)
                return NotFound();

            activityRepo.Log("Manager", "Deleted Organizer", organizer.FullName);

            var theirEvents = eventRepo.GetEventsByOrganizer(id);
            foreach (var ev in theirEvents)
            {
                eventRepo.RemoveEvent(ev);
            }

            if (repo.RemoveOrganizer(organizer))
                return Ok();

            return BadRequest();
        }

        // PUT api/Organizer/5/toggle-active
        [HttpPut("{id}/toggle-active")]
        public ActionResult ToggleActive(int id)
        {
            var organizer = repo.GetOrganizerByID(id);
            if (organizer == null) return NotFound();
            organizer.IsActive = !organizer.IsActive;
            repo.UpdateOrganizer(organizer);
            activityRepo.Log("Manager", organizer.IsActive ? "Reactivated Organizer" : "Suspended Organizer", organizer.FullName);
            return Ok(organizer.IsActive);
        }
    }
}