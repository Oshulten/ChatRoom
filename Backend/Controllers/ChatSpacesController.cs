using Backend.Models;
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

        [HttpPost]
        public ActionResult Post(ChatSpacePost post)
        {
            var space = (ChatSpace)post;
            db.ChatSpaces.Add(space);
            db.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = space.Id }, null);
        }
    }
}