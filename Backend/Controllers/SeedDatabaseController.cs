using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
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
            new(amandaId, "Hello world!", globalId),
            new(amandaId, "How is everyone doing?", globalId),
            new(michaelId, "It's raining...", globalId),
            new(georgeId, "Then let's dance in the rain!", globalId),

            new(amandaId, "This is my private space", amandasCornerId),
            new(amandaId, "It's kinda quiet in here...", amandasCornerId),
            new(amandaId, "Should I invite George?", amandasCornerId),
            new(georgeId, "Thanks for sending an invite!", amandasCornerId),
            new(amandaId, "Just don't tell Michael", amandasCornerId),
        };

        db.ChatMessages.AddRange(defaultMessages);
        db.ChatSpaces.AddRange(defaultChatSpaces);
        db.ChatUsers.AddRange(defaultUsers);

        db.SaveChanges();
        return Ok();
    }
}