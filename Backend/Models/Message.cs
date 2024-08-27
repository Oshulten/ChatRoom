using Backend.Database;
using Backend.Dto;

namespace Backend.Models;

public class Message(User sender, DateTime postedAt, string content, Space space)
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public User Sender { get; set; } = sender;
    public DateTime PostedAt { get; set; } = postedAt;
    public string Content { get; set; } = content;
    public Space Space { get; set; } = space;

    public static readonly Message Null = new(User.Null, DateTime.Now, string.Empty, Space.Null);

    public Message() : this(Null.Sender, Null.PostedAt, Null.Content, Null.Space) { }

    public static Message MessageFromDtoMessage(DtoMessagePost dto, ChatroomDatabaseContext context)
    {
        var user = context.Users.FirstOrDefault(user => user.Guid == dto.SenderGuid)
            ?? throw new Exception("User guid on dto must map to an existing user");

        var space = context.Spaces.FirstOrDefault(space => space.Guid == dto.SpaceGuid)
            ?? throw new Exception("Space guid on dto must map to an existing space");

        return new(user, DateTime.Now, dto.Content, space);
    }

    public static explicit operator DtoMessage(Message post) =>
        new(post.Content, post.Space.Guid, post.Sender.Guid, post.PostedAt);
}