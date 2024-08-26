using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dto;

public record DtoSpacePost
(
    [Length(Restrictions.AliasMinLength, Restrictions.AliasMaxLength)]
    string Alias
)
{ public DtoSpacePost() : this(string.Empty) { } };