using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto;

public record DtoUser(
    Guid Guid,
    string Alias,
    DateTime JoinedAt,
    bool Admin
);