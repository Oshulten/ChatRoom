using Backend.Models;
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
        if (!db.ChatUsers.Any())
        {
            db.ChatUsers!.AddRange(ChatUser.SeedData());
        }
        db.SaveChanges();
        return db.ChatUsers.ToList()!;
    }

    [HttpGet("after-date/{date}")]
    public IEnumerable<ChatUser> GetAfterDate(DateTime date)
    {
        return db.ChatUsers.Where(user => user.JoinedAt.CompareTo(date) > 0)
                           .OrderBy(user => user.JoinedAt);
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

    // [HttpPost]
    // public IActionResult Post(DefaultDataTypeView dto, CustomDatabaseContext db)
    // {
    //     var entity = (DefaultDataType)dto;
    //     db.Add<DefaultDataType>(entity);
    //     db.SaveChanges();
    //     return CreatedAtAction(nameof(GetById), new { id = entity.Id }, null);
    // }

    // [HttpDelete("{id}")]
    // public IActionResult DeleteById(Guid id, CustomDatabaseContext db)
    // {
    //     DefaultDataType? entity = db.DefaultDataTable.FirstOrDefault(data => data.Id == id);
    //     if (entity is null)
    //     {
    //         return NotFound();
    //     }
    //     db.Remove<DefaultDataType>(entity);
    //     db.SaveChanges();
    //     return Ok();
    // }

    // [HttpDelete]
    // public IActionResult DeleteAll(string deleteAllKey, CustomDatabaseContext db)
    // {
    //     if (deleteAllKey != _deleteAllKey)
    //     {
    //         return Unauthorized();
    //     }

    //     db.RemoveRange(db.DefaultDataTable.ToList());
    //     db.SaveChanges();
    //     return Ok();
    // }
}