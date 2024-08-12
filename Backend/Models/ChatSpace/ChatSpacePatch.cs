using System.ComponentModel.DataAnnotations;

namespace Backend.Models.ChatSpace;

public record ChatSpacePatch(
    [Length(Validation.AliasMinLength, Validation.AliasMaxLength)]
    string? Alias,

    Guid[] UserIds
);