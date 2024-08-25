using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpacesController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<DbSpace> GetByUser(Guid userGuid)
        {
            return db.Spaces.Where(space => space.Members.Select(member => member.Guid).Contains(userGuid));
        }
    }
}