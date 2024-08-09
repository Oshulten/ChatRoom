using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record ChatSpacePost
    (
        [Length(Validation.AliasMinLength, Validation.AliasMinLength)]
        string Alias,
        [Length(Validation.PasswordMinLength, Validation.PasswordMinLength)]
        string Password
    );
}