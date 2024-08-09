using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Validation
    {
        public const int AliasMinLength = 3;
        public const int AliasMaxLength = 25;
        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 25;
        public const int ContentMinLength = 1;
        public const int ContentMaxLength = 250;
    }


}