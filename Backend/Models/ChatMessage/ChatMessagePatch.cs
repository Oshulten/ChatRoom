using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.ChatMessage;

public record ChatMessagePatch(
    Guid? UserId,
    DateTime? PostedAt,
    string? Content,
    Guid? ChatSpaceId
);