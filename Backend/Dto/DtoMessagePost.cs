namespace Backend.Dto;

public record DtoMessagePost
(
    string Content,
    Guid SpaceGuid,
    Guid SenderGuid
);