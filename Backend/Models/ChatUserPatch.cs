using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record ChatUserPatch
    (
        [Length(3, 25)]
        string? Alias,

        [Length(8, 25)]
        string? Password,

        bool? Admin
    );
}