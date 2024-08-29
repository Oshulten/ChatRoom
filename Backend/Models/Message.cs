using Backend.Database;
using Backend.Dto;

namespace Backend.Models;

public class Message(User sender, DateTime postedAt, string content, Space space)
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public User Sender { get; set; } = sender;
    public DateTime PostedAt { get; set; } = postedAt;
    public string Content { get; set; } = content;
    public Space Space { get; set; } = space;

    public static readonly Message Null = new(User.Null, DateTime.Now, string.Empty, Space.Null);

    public Message() : this(Null.Sender, Null.PostedAt, Null.Content, Null.Space) { }
}