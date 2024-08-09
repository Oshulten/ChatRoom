using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public record ChatUserPatch
    (
        [Length(Validation.AliasMinLength, Validation.AliasMaxLength)]
        string? Alias,

        [Length(Validation.PasswordMinLength, Validation.PasswordMaxLength)]
        string? Password,

        bool? Admin
    );
}