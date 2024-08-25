using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.User;

public record DtoUser(
    Guid Guid,
    string Alias,
    DateTime JoinedAt,
    bool Admin
);