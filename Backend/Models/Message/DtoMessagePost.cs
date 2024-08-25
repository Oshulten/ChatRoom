namespace Backend.Models.Message;

public record DtoMessagePost
(
    string Content,
    Guid SpaceGuid,
    Guid SenderGuid
);