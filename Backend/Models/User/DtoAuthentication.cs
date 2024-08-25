using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.User;

public record DtoAuthentication(
    string Alias,
    string Password
);
