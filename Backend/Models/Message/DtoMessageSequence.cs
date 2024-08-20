using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Message;

public record DtoMessageSequence
(
    DateTime FromDate,
    DateTime ToDate,
    bool Earliest,
    List<DtoMessage> Messages
);