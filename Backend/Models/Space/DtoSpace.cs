using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Space;

public record DtoSpace
(
    [Length(Validation.AliasMinLength, Validation.AliasMaxLength)]
    string Alias,
    List<Guid> MemberGuids
);