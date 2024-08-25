using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto;

public record DtoMessageSequenceRequest
(
    Guid SpaceGuid,
    DateTime MessagesBefore,
    int NumberOfMessages
);