using Microsoft.AspNetCore.Mvc;
using KMC_API.Data;
using KMC_API.DTO;
using KMC_API.Helpers;
using AutoMapper;

namespace KMC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private IMapper mapper;
        private ManagerRepo repo;
        private ActivityLogRepo activityRepo;

        public ManagerController(IMapper _mapper, ManagerRepo _repo, ActivityLogRepo _activityRepo)
        {
            mapper = _mapper;
            repo = _repo;
            activityRepo = _activityRepo;
        }

        [HttpPost("login")]
        public ActionResult<ManagerReadDTO> Login(ManagerLoginDTO dto)
        {
            var manager = repo.GetManagerByUsername(dto.Username);
            if (manager == null || !PasswordHelper.Verify(dto.Password, manager.PasswordHash))
                return Unauthorized("Invalid username or password.");

            return Ok(mapper.Map<ManagerReadDTO>(manager));
        }

        // GET api/Manager/dashboard-stats?managerId=1
        [HttpGet("dashboard-stats")]
        public ActionResult<DashboardStatsDTO> GetDashboardStats(int managerId)
        {
            if (repo.GetManagerByID(managerId) == null)
                return Forbid();

            var stats = repo.GetPlatformStats();
            var breakdown = repo.GetCategoryBreakdown();

            return Ok(new DashboardStatsDTO
            {
                TotalOrganizers = stats.totalOrganizers,
                TotalEvents = stats.totalEvents,
                TotalRegistrations = stats.totalRegistrations,
                TotalSeatsBooked = stats.totalSeats,
                TotalRevenue = stats.totalRevenue,
                EventsByCategory = breakdown.Select(b => new CategoryStatDTO
                {
                    CategoryName = b.category,
                    EventCount = b.eventCount,
                    TotalRevenue = b.revenue
                }).ToList()
            });
        }

        [HttpGet("activity-log")]
        public ActionResult<List<ActivityLogReadDTO>> GetActivityLog(int managerId)
        {
            if (repo.GetManagerByID(managerId) == null)
                return Forbid();

            var logs = activityRepo.GetRecent(10);
            return Ok(mapper.Map<List<ActivityLogReadDTO>>(logs));
        }
    }
}