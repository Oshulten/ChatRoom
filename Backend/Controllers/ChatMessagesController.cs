using Backend.Models;
using Backend.Models.ChatMessage;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatMessagesController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet("{id}")]
    public ChatMessage GetById(Guid id)
    {
        return db.ChatMessages.FirstOrDefault(data => data.Id == id)!;
    }

    [HttpGet]
    public IEnumerable<ChatMessage> GetAll()
    {
        return db.ChatMessages;
    }

    [HttpGet("get-first")]
    public ChatMessage GetFirst()
    {
        return db.ChatMessages.FirstOrDefault()!;
    }

    [HttpPatch("{id}")]
    public IActionResult PatchById(Guid id, ChatMessagePatch patchObject)
    {
        ChatMessage? entity = db.ChatMessages.FirstOrDefault(data => data.Id == id);

        if (entity is null)
        {
            return NotFound();
        }

        entity.Patch(patchObject);
        db.SaveChanges();
        return Ok();
    }
}