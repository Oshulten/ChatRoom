namespace Backend.Models.ChatMessage;

public class ChatMessage(Guid userId, DateTime postedAt, string content, Guid chatSpace)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
    public DateTime PostedAt { get; set; } = postedAt;
    public string Content { get; set; } = content;
    public Guid ChatSpaceId { get; set; } = chatSpace;

    public ChatMessage() : this(Guid.NewGuid(), DateTime.Now, "A message", Guid.NewGuid()) { }

    public static explicit operator ChatMessage(ChatMessagePost post) => new(
        post.ChatUserId, DateTime.Now, post.Content, post.ChatSpaceId
    );

    public void Patch(ChatMessagePatch patchObject)
    {
        UserId = patchObject.UserId ?? UserId;
        PostedAt = patchObject.PostedAt ?? PostedAt;
        Content = patchObject.Content ?? Content;
        ChatSpaceId = patchObject.ChatSpaceId ?? ChatSpaceId;
    }
}