using Backend.Models;
using Backend.Models.Message;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessageSequence))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoMessageSequence> GetBySpaceAndDate(Guid spaceId, DateTime date, int numberOfMessages)
    {
        Console.WriteLine($"spaceId {spaceId}, date {date}, numberOfMessages {numberOfMessages}");
        var ids = db.Messages.Select(m => m.SpaceId);
        foreach (var id in ids) Console.WriteLine(id);
        var orderedMessages = db.Messages
                                .Where(message => message.SpaceId == spaceId)
                                .OrderByDescending(message => message.PostedAt).ToList()
                                .Select(message => (DtoMessage)message).ToList();
        var messages = orderedMessages.Take(numberOfMessages).ToList();

        if (messages.Count == 0)
        {
            Console.Write("Not found");
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