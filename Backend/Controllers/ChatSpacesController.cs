using Backend.Models;
using Backend.Models.ChatSpace;
using Backend.Models.ChatUser;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatSpacesController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpGet("{id}")]
        public ChatSpace GetById(Guid id)
        {
            return db.ChatSpaces.FirstOrDefault(data => data.Id == id)!;
        }

        [HttpGet]
        public IEnumerable<ChatSpace> GetAll()
        {
            return db.ChatSpaces;
        }

        [HttpGet("get-first")]
        public ChatSpace GetFirst()
        {
            return db.ChatSpaces.FirstOrDefault()!;
        }

        [HttpPatch("{id}")]
        public IActionResult PatchById(Guid id, ChatSpacePatch patchObject)
        {
            ChatSpace? entity = db.ChatSpaces.FirstOrDefault(data => data.Id == id);

            if (entity is null)
            {
                return NotFound();
            }

            entity.Patch(patchObject);
            db.SaveChanges();
            return Ok();
        }
    }
}