using Backend.Models;
using Backend.Models.ChatMessage;
using Backend.Models.ChatSpace;
using Backend.Models.ChatUser;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SeedDatabaseController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpPost]
    public IActionResult SeedData()
    {
        db.ChatMessages.RemoveRange(db.ChatMessages);
        db.ChatSpaces.RemoveRange(db.ChatSpaces);
        db.ChatUsers.RemoveRange(db.ChatUsers);

        var defaultUsers = new List<ChatUser>() {
            new("Michael", "Password", true, new DateTime(1983, 4, 15)),
            new("Amanda", "Password", false, new DateTime(1991, 5, 2)),
            new("George", "Password", false, new DateTime(2022, 11, 18)),
        };

        var allUserIds = defaultUsers.Select(user => user.Id);
        var michaelId = defaultUsers[0].Id;
        var amandaId = defaultUsers[1].Id;
        var georgeId = defaultUsers[2].Id;

        var defaultChatSpaces = new List<ChatSpace>() {
            new("Global", allUserIds.ToArray()),
            new("Amanda's Corner", [amandaId, georgeId])
        };

        var globalId = defaultChatSpaces[0].Id;
        var amandasCornerId = defaultChatSpaces[1].Id;

        var defaultMessages = new List<ChatMessage>() {
            new(amandaId, new DateTime(2021, 04, 5), "Hello world!", globalId),
            new(amandaId, new DateTime(2021, 04, 6), "How is everyone doing?", globalId),
            new(michaelId, new DateTime(2021, 04, 7), "It's raining...", globalId),
            new(georgeId, new DateTime(2021, 04, 8), "Then let's dance in the rain!", globalId),

            new(amandaId, new DateTime(2021, 04, 5), "This is my private space", amandasCornerId),
            new(amandaId, new DateTime(2021, 04, 6), "It's kinda quiet in here...", amandasCornerId),
            new(amandaId, new DateTime(2021, 04, 7), "Should I invite George?", amandasCornerId),
            new(georgeId, new DateTime(2021, 04, 8), "Thanks for sending an invite!", amandasCornerId),
            new(amandaId, new DateTime(2021, 04, 9), "Just don't tell Michael", amandasCornerId),
        };

        db.ChatMessages.AddRange(defaultMessages);
        db.ChatSpaces.AddRange(defaultChatSpaces);
        db.ChatUsers.AddRange(defaultUsers);

        db.SaveChanges();
        return Ok();
    }
}