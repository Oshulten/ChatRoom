using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto;

public record DtoAuthentication(
    string Alias,
    string Password
);
