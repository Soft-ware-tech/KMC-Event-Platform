using Microsoft.AspNetCore.Mvc;
using KMC_API.Model;
using KMC_API.Data;
using KMC_API.DTO;
using AutoMapper;

namespace KMC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IMapper mapper;
        private CategoryRepo repo;
        private ManagerRepo managerRepo;
        private ActivityLogRepo activityRepo;

        public CategoryController(IMapper _mapper, CategoryRepo _repo, ManagerRepo _managerRepo, ActivityLogRepo _activityRepo)
        {
            mapper = _mapper;
            repo = _repo;
            managerRepo = _managerRepo;
            activityRepo = _activityRepo;
        }

        // GET api/Category
        [HttpGet]
        public ActionResult<List<CategoryReadDTO>> GetAll()
        {
            var categories = repo.GetAll();
            return Ok(mapper.Map<List<CategoryReadDTO>>(categories));
        }

        // POST api/Category?managerId=1
        [HttpPost]
        public ActionResult<CategoryReadDTO> Add(CategoryCreateDTO dto, int managerId)
        {
            if (managerRepo.GetManagerByID(managerId) == null)
                return Forbid();

            if (repo.NameExists(dto.Name))
                return Conflict("This category already exists.");

            var category = mapper.Map<Category>(dto);
            activityRepo.Log("Manager", "Added Category", category.Name);

            if (repo.Add(category))
                return Ok(mapper.Map<CategoryReadDTO>(category));

            return BadRequest();
        }

        // DELETE api/Category/5?managerId=1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id, int managerId)
        {
            if (managerRepo.GetManagerByID(managerId) == null)
                return Forbid();

            var category = repo.GetById(id);
            if (category == null)
                return NotFound();

            activityRepo.Log("Manager", "Deleted Category", category.Name);

            if (repo.Remove(category))
                return Ok();

            return BadRequest();
        }
    }
}