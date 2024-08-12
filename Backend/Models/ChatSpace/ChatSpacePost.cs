using System.ComponentModel.DataAnnotations;

namespace Backend.Models.ChatSpace;

public record ChatSpacePost
(
    [Length(Validation.AliasMinLength, Validation.AliasMaxLength)]
    string Alias,

    Guid[] UserIds
);