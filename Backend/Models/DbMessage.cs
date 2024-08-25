using Backend.Database;
using Backend.Dto;

namespace Backend.Models;

public class DbMessage(DbUser sender, DateTime postedAt, string content, DbSpace space)
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public DbUser Sender { get; set; } = sender;
    public DateTime PostedAt { get; set; } = postedAt;
    public string Content { get; set; } = content;
    public DbSpace Space { get; set; } = space;

    public static readonly DbMessage Null = new(DbUser.Null, DateTime.Now, string.Empty, DbSpace.Null);

    public DbMessage() : this(Null.Sender, Null.PostedAt, Null.Content, Null.Space) { }

    public static DbMessage MessageFromDtoMessage(DtoMessagePost dto, ChatroomDatabaseContext context)
    {
        var user = context.Users.FirstOrDefault(user => user.Guid == dto.SenderGuid)
            ?? throw new Exception("User guid on dto must map to an existing user");

        var space = context.Spaces.FirstOrDefault(space => space.Guid == dto.SpaceGuid)
            ?? throw new Exception("Space guid on dto must map to an existing space");

        return new(user, DateTime.Now, dto.Content, space);
    }

    public static explicit operator DtoMessage(DbMessage post) =>
        new(post.Content, post.Space.Guid, post.Sender.Guid, post.PostedAt);
}