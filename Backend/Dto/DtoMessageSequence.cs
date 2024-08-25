namespace Backend.Dto;

public record DtoMessageSequence
(
    DateTime FromDate,
    DateTime ToDate,
    bool Earliest,
    List<DtoMessage> Messages,
    List<DtoUser> Senders
);