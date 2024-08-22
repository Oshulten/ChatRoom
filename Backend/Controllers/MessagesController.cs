using Backend.Models;
using Backend.Models.Message;
using Backend.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessageSequence))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoMessageSequence> GetBySpaceAndDate(Guid spaceId, DateTime messagesBefore, int numberOfMessages)
    {
        var messages = db.Messages
                            .Where(message => message.SpaceId == spaceId)
                            .OrderByDescending(message => message.PostedAt)
                            // .Where(message => message.PostedAt < messagesBefore)
                            .Take(numberOfMessages)
                            .Select(message => (DtoMessage)message)
                            .ToList();

        if (messages.Count == 0)
        {
            return NotFound();
        }

        var distinctUsers = messages
                        .Select(message => message.UserId)
                        .Distinct()
                        .Select(id => db.Users.FirstOrDefault(user => user.Id == id));

        if (distinctUsers.Any(user => user is null))
        {
            return Problem("Corrupt database");
        }

        var dtoUsers = distinctUsers.Select(user => (DtoUser)user!);

        var earliestMessage = messages[0];
        var lastMessage = messages[^1];
        var earliest = earliestMessage == messages[0];

        return new DtoMessageSequence(
            earliestMessage.PostedAt,
            lastMessage.PostedAt,
            earliest,
            messages,
            dtoUsers.ToList()
        );
    }
}