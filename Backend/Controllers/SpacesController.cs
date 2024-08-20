using Backend.Models;
using Backend.Models.Space;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpacesController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<DbSpace> GetByUser(Guid userId)
        {
            return db.Spaces.Where(space => space.UserIds.Contains(userId));
        }
    }
}