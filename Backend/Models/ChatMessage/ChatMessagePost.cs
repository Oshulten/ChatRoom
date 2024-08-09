using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record ChatMessagePost
    (
        [Length(Validation.ContentMinLength, Validation.ContentMaxLength)]
        string Content,
        Guid ChatSpace,
        Guid ChatUser
    );
}