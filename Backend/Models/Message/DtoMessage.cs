using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Message;

public record DtoMessage
(
    string Content,
    Guid ChatSpaceId,
    Guid ChatUserId,
    DateTime PostedAt
);