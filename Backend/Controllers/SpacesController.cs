using Backend.Database;
using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpacesController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<DtoSpace> GetByUserGuid(Guid userGuid)
        {
            return db.SpacesByUserGuidMembership(userGuid)
                .Select(space => (DtoSpace)space);
        }

        [HttpGet("{spaceGuid}")]
        public ActionResult<DtoSpace> GetByGuid(string spaceGuid)
        {
            var guid = Guid.Parse(spaceGuid);
            var space = db.SpaceByGuid(guid);

            if (space is null)
            {
                return NotFound(SpaceNotFound(guid));
            }

            return (DtoSpace)db.SpaceByGuid(guid)!;
        }

        [HttpPost]
        public ActionResult<DtoSpace> PostSpace(DtoSpacePost space)
        {
            var dbSpace = (DbSpace)space;

            db.Spaces.Add(dbSpace);
            db.SaveChanges();

            var dto = (DtoSpace)dbSpace;
            return CreatedAtAction(null, dto);
        }

        [HttpPut("{spaceGuid}")]
        public IActionResult AddExistingMemberToSpace(Guid spaceGuid, DtoUser member)
        {
            var existingSpace = db.SpaceByGuid(spaceGuid);

            if (existingSpace is null)
                return NotFound(SpaceNotFound(spaceGuid));

            var existingUser = db.Users.FirstOrDefault(user => user.Guid == member.Guid);

            if (existingUser is null)
                return NotFound($"A user with guid {member.Guid} doesn't exist");

            existingSpace.Members.Add(existingUser);

            db.SaveChanges();

            return Ok();
        }
        private static string SpaceNotFound(Guid spaceGuid) =>
            $"A space with guid {spaceGuid} doesn't exist";
    }
}