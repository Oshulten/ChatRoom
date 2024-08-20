using Backend.Models;
using Backend.Models.Message;
using Backend.Models.Space;
using Backend.Models.User;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SeedDatabaseController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpPost]
    public IActionResult SeedData()
    {
        db.Messages.RemoveRange(db.Messages);
        db.Spaces.RemoveRange(db.Spaces);
        db.Users.RemoveRange(db.Users);

        var defaultUsers = new List<DbUser>() {
            new("Michael", "Password", true, new DateTime(1983, 4, 15)),
            new("Amanda", "Password", false, new DateTime(1991, 5, 2)),
            new("George", "Password", false, new DateTime(2022, 11, 18)),
        };

        var allUserIds = defaultUsers.Select(user => user.Id);
        var michaelId = defaultUsers[0].Id;
        var amandaId = defaultUsers[1].Id;
        var georgeId = defaultUsers[2].Id;

        var defaultChatSpaces = new List<DbSpace>() {
            new("Global", allUserIds.ToArray()),
            new("Amanda's Corner", [amandaId, georgeId])
        };

        var globalId = defaultChatSpaces[0].Id;
        var amandasCornerId = defaultChatSpaces[1].Id;

        var defaultMessages = new List<DbMessage>() {
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

        db.Messages.AddRange(defaultMessages);
        db.Spaces.AddRange(defaultChatSpaces);
        db.Users.AddRange(defaultUsers);

        db.SaveChanges();
        return Ok();
    }

    [HttpPost("bogus")]
    public IActionResult SeedDataWithBogus(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        db.Messages.RemoveRange(db.Messages);
        db.Spaces.RemoveRange(db.Spaces);
        db.Users.RemoveRange(db.Users);

        var users = new Faker<DbUser>()
            .RuleFor(o => o.Alias, f => f.Person.UserName)
            .RuleFor(o => o.Password, f => f.Internet.Password(10, true))
            .RuleFor(o => o.JoinedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Admin, f => f.Random.Bool())
            .Generate(numberOfUsers);

        users.Add(new DbUser()
        {
            Alias = "qwer",
            Password = "qwer",
            JoinedAt = new DateTime(1991, 04, 26),
            Admin = true
        });

        var userIds = users.Select(user => user.Id).ToArray();

        var spaces = new Faker<DbSpace>()
            .RuleFor(o => o.Alias, f => f.Internet.DomainWord())
            .RuleFor(o => o.UserIds, f => f.Random.ArrayElements<Guid>(userIds))
            .Generate(numberOfSpaces);

        var spaceIds = spaces.Select(space => space.Id).ToArray();

        var messages = new Faker<DbMessage>()
            .RuleFor(o => o.UserId, f => f.Random.ListItem<Guid>(userIds))
            .RuleFor(o => o.PostedAt, f => f.Date.Between(new DateTime(1991, 04, 26), DateTime.Now))
            .RuleFor(o => o.Content, f => f.Random.Words(Random.Shared.Next(3, 50)))
            .RuleFor(o => o.SpaceId, f => f.Random.ArrayElement(userIds))
            .Generate(numberOfMessages);

        db.Messages.AddRange(messages);
        db.Spaces.AddRange(spaces);
        db.Users.AddRange(users);

        db.SaveChanges();
        return Ok();
    }
}