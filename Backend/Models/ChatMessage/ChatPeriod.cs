using System.ComponentModel.DataAnnotations;

namespace Backend.Models.ChatMessage;
public class ChatPeriod
{
    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
    public required bool Earliest { get; set; }
    public required List<ChatMessagePost> Messages { get; set; }
}