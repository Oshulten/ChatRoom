using Backend.Models;
using Backend.Models.Message;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoMessageSequence> GetBySpaceAndDate(Guid spaceId, DateTime date, int numberOfMessages)
    {
        var orderedMessages = db.Messages
                                .Where(message =>
                                    message.SpaceId == spaceId &&
                                    message.PostedAt < date)
                                .OrderByDescending(message => message.PostedAt)
                                .Select(message => (DtoMessage)message).ToList();

        var messages = orderedMessages.Take(numberOfMessages).ToList();

        if (messages.Count == 0)
        {
            return NotFound();
        }

        var earliestMessage = messages[0];
        var lastMessage = messages[^1];
        var earliest = earliestMessage == orderedMessages[0];

        return new DtoMessageSequence(
            earliestMessage.PostedAt,
            lastMessage.PostedAt,
            earliest,
            messages
        );
    }
}