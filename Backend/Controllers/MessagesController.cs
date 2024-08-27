using Backend.Database;
using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(ChatroomDatabaseContext context) : ControllerBase
{
    [HttpGet("{spaceGuid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessageSequence))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoMessageSequence> GetBySpaceAndDate(Guid spaceGuid, [FromQuery] DateTime? messagesBefore, [FromQuery] int? numberOfMessages)
    {
        if (context.SpaceByGuid(spaceGuid) is null)
        {
            return NotFound($"A space with guid {spaceGuid} doesn't exist");
        }

        var messages = context.Messages
            .Where(message => message.Space.Guid == spaceGuid)
            .OrderByDescending(message => message.PostedAt)
            .ToList();

        if (messagesBefore is not null)
        {
            messages = messages.Where(message => message.PostedAt < messagesBefore).ToList();
        }

        if (numberOfMessages is not null)
        {
            messages = messages.Take(numberOfMessages ?? 0).ToList();
        }

        var dtoMessages = messages
            .Select(message => (DtoMessage)message)
            .ToList();

        var distinctUsers = dtoMessages
            .Select(message => message.SenderGuid)
            .Distinct()
            .Select(guid => context.Users.FirstOrDefault(user => user.Guid == guid));

        if (distinctUsers.Any(user => user is null))
        {
            return Problem("Corrupt database");
        }

        var dtoUsers = distinctUsers.Select(user => (DtoUser)user!);

        var earliestMessage = dtoMessages[^1];
        var lastMessage = dtoMessages[0];
        var earliest = earliestMessage == dtoMessages[0];

        if (numberOfMessages is null)
        {
            earliest = true;
        }
        else
        {
            earliest = dtoMessages.Count < numberOfMessages;
        }

        return new DtoMessageSequence(
            earliestMessage.PostedAt,
            lastMessage.PostedAt,
            earliest,
            dtoMessages,
            dtoUsers.ToList()
        );
    }

    [HttpPost]
    public ActionResult<DtoMessage> PostMessage(DtoMessagePost post)
    {
        var createdMessage = Message.MessageFromDtoMessage(post, context);
        context.Messages.Add(createdMessage);
        context.SaveChanges();
        return (DtoMessage)createdMessage;
    }
}