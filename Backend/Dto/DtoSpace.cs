using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dto;

public record DtoSpace
(
    [Length(Restrictions.AliasMinLength, Restrictions.AliasMaxLength)]
    string Alias,
    Guid Guid,
    List<Guid> MemberGuids
);