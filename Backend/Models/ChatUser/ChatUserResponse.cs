using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.ChatUser
{
    public record ChatUserResponse(
        Guid Id,
        string Alias,
        DateTime JoinedAt,
        bool Admin
    );
}