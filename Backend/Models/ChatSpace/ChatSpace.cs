namespace Backend.Models.ChatSpace;

public class ChatSpace(string alias, Guid[] userIds)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Alias { get; set; } = alias;

    public Guid[] UserIds { get; set; } = userIds;

    public ChatSpace() : this("An empty room", [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()]) { }

    public static explicit operator ChatSpace(ChatSpacePost post) => new(
        post.Alias, post.UserIds
    );

    public void Patch(ChatSpacePatch patchObject)
    {
        Alias = patchObject.Alias ?? Alias;
        UserIds = patchObject.UserIds ?? UserIds;
    }
}