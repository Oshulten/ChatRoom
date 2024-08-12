using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.ChatUser;

public record LoginRequest(
    string Username,
    string Password
);
