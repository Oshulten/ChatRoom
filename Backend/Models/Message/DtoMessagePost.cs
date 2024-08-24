namespace Backend.Models.Message;

public record DtoMessagePost
(
    string Content,
    Guid SpaceId,
    Guid UserId
);