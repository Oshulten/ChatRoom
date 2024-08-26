using Backend.Database;
using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(ChatroomDatabaseContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessageSequence))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoMessageSequence> GetBySpaceAndDate([FromQuery] Guid spaceGuid, [FromQuery] DateTime messagesBefore, [FromQuery] int numberOfMessages)
    {
        var messages = context.Messages
            .Where(message => message.Space.Guid == spaceGuid)
            .OrderByDescending(message => message.PostedAt)
            .Where(message => message.PostedAt < messagesBefore)
            .Take(numberOfMessages)
            .Select(message => (DtoMessage)message)
            .ToList();

        if (messages.Count == 0)
        {
            return NotFound();
        }

        var distinctUsers = messages
            .Select(message => message.SenderGuid)
            .Distinct()
            .Select(guid => context.Users.FirstOrDefault(user => user.Guid == guid));

        if (distinctUsers.Any(user => user is null))
        {
            return Problem("Corrupt database");
        }

        var dtoUsers = distinctUsers.Select(user => (DtoUser)user!);

        var earliestMessage = messages[^1];
        var lastMessage = messages[0];
        var earliest = earliestMessage == messages[0];
        if (messages.Count < numberOfMessages) earliest = true;

        return new DtoMessageSequence(
            earliestMessage.PostedAt,
            lastMessage.PostedAt,
            earliest,
            messages,
            dtoUsers.ToList()
        );
    }

    [HttpPost]
    public ActionResult<DtoMessage> PostMessage(DtoMessagePost post)
    {
        var createdMessage = DbMessage.MessageFromDtoMessage(post, context);
        context.Messages.Add(createdMessage);
        context.SaveChanges();
        return (DtoMessage)createdMessage;
    }
}