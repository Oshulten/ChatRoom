using Backend.Models;
using Backend.Models.ChatMessage;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatMessagesController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet]
    public ChatPeriod GetBySpaceAndDate(Guid spaceId, DateTime date, int numberOfMessages)
    {
        var orderedMessages = db.ChatMessages
                                .Where(message =>
                                    message.ChatSpaceId == spaceId &&
                                    message.PostedAt < date)
                                .OrderByDescending(message => message.PostedAt)
                                .Select(message => (ChatMessagePost)message).ToList();
        var messages = orderedMessages.Take(numberOfMessages).ToList();
        var earliestMessage = messages[0];
        var lastMessage = messages[^1];
        var earliest = earliestMessage == orderedMessages[0];

        return new ChatPeriod()
        {
            Earliest = earliest,
            FromDate = earliestMessage.PostedAt,
            ToDate = lastMessage.PostedAt,
            Messages = messages
        };
    }
}