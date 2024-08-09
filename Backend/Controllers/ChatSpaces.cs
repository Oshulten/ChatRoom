using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatSpaces(ChatroomDatabaseContext db) : ControllerBase
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
    }
}