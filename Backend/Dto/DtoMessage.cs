namespace Backend.Dto;

public record DtoMessage
(
    string Content,
    Guid SpaceGuid,
    Guid SenderGuid,
    DateTime PostedAt
);