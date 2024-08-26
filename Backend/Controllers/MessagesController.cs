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
    public ActionResult<DtoMessageSequence> GetBySpaceAndDate(DtoGetBySpaceAndDate dto)
    {
        if (context.SpaceByGuid(dto.SpaceGuid) is null)
        {
            return NotFound($"A space with guid {dto.SpaceGuid} doesn't exist");
        }

        var messages = context.Messages
            .Where(message => message.Space.Guid == dto.SpaceGuid)
            .OrderByDescending(message => message.PostedAt)
            .ToList();

        if (dto.MessagesBefore is not null)
        {
            messages = messages.Where(message => message.PostedAt < dto.MessagesBefore).ToList();
        }

        if (dto.NumberOfMessages is not null)
        {
            messages = messages.Take(dto.NumberOfMessages ?? 0).ToList();
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

        if (dto.NumberOfMessages is null)
        {
            earliest = true;
        }
        else
        {
            earliest = dtoMessages.Count < dto.NumberOfMessages;
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
        var createdMessage = DbMessage.MessageFromDtoMessage(post, context);
        context.Messages.Add(createdMessage);
        context.SaveChanges();
        return (DtoMessage)createdMessage;
    }
}