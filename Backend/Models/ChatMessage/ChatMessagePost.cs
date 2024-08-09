using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record ChatMessagePost
    (
        Guid UserId,
        [Length(Validation.ContentMinLength, Validation.ContentMinLength)]
        string Content,
        Guid ChatSpace
    );
}