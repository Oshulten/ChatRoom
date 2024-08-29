using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto;
using Bogus;

namespace Backend.Tests.Utilities;

public static class DtoGenerator
{
    public static DtoAuthentication GenerateDtoAuthentication() =>
        new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

    public static DtoSpacePost GenerateDtoSpacePost() =>
        new(Guid.NewGuid().ToString());
}