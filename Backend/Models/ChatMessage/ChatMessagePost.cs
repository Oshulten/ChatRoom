using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.ChatMessage;

public record ChatMessagePost
(
    [Length(Validation.ContentMinLength, Validation.ContentMaxLength)]
    string Content,
    [Required]
    Guid ChatSpaceId,
    [Required]
    Guid ChatUserId,
    [Required]
    DateTime PostedAt
);