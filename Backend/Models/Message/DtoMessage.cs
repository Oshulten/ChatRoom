namespace Backend.Models.Message;

public record DtoMessage
(
    string Content,
    Guid SpaceGuid,
    Guid SenderGuid,
    DateTime PostedAt
);