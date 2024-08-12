using System.ComponentModel.DataAnnotations;

namespace Backend.Models.ChatUser;

public record ChatUserPost
(
    [Length(Validation.AliasMinLength, Validation.AliasMaxLength)]
    string Alias,

    [Length(Validation.PasswordMinLength, Validation.PasswordMaxLength)]
    string Password,

    DateTime JoinedAt,
    bool Admin
);