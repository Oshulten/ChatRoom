using Backend.Models;
using Backend.Models.ChatUser;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatUsersController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet("{id}")]
    public ChatUser GetById(Guid id)
    {
        return db.ChatUsers.FirstOrDefault(data => data.Id == id)!;
    }

    [HttpGet]
    public List<ChatUser> GetAll()
    {
        db.SaveChanges();
        return db.ChatUsers.ToList()!;
    }

    [HttpPost("login")]
    public ChatUser Login(LoginRequest loginRequest)
    {
        ChatUser? user = db.ChatUsers.FirstOrDefault(user => user.Alias == loginRequest.Username && user.Password == loginRequest.Password);
        return user!;
    }

    [HttpGet("get-first")]
    public ChatUser GetFirst()
    {
        return db.ChatUsers.FirstOrDefault()!;
    }

    [HttpGet("get-by-space/{spaceId}")]
    public IEnumerable<ChatUser> GetBySpace(Guid spaceId)
    {
        var space = db.ChatSpaces.First(space => space.Id == spaceId);
        return db.ChatUsers.Where(user => space.UserIds.Contains(user.Id));
    }

    [HttpPatch("{id}")]
    public IActionResult PatchById(Guid id, ChatUserPatch patchObject)
    {
        ChatUser? entity = db.ChatUsers.FirstOrDefault(data => data.Id == id);

        if (entity is null)
        {
            return NotFound();
        }

        entity.Patch(patchObject);
        db.SaveChanges();
        return Ok();
    }
}