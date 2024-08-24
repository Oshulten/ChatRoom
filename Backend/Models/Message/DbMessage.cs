namespace Backend.Models.Message;

public class DbMessage(Guid userId, DateTime postedAt, string content, Guid chatSpace)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
    public DateTime PostedAt { get; set; } = postedAt;
    public string Content { get; set; } = content;
    public Guid SpaceId { get; set; } = chatSpace;

    public DbMessage() : this(Guid.NewGuid(), DateTime.Now, "A message", Guid.NewGuid()) { }

    public static explicit operator DbMessage(DtoMessage post) => new(
        post.UserId, DateTime.Now, post.Content, post.SpaceId
    );

    public static explicit operator DbMessage(DtoMessagePost post) => new(
        post.UserId, DateTime.Now, post.Content, post.SpaceId
    );

    public static explicit operator DtoMessage(DbMessage post) => new(
        post.Content, post.SpaceId, post.UserId, post.PostedAt
    );

}