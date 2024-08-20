namespace Backend.Models.Space;

public class DbSpace(string alias, Guid[] userIds)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Alias { get; set; } = alias;
    public Guid[] UserIds { get; set; } = userIds;

    public DbSpace() : this("An empty room", [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()]) { }

    public static explicit operator DbSpace(DtoSpace post) => new(post.Alias, post.UserIds);
}