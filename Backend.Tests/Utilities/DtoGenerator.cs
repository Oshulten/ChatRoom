using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto;
using Bogus;

namespace Backend.Tests.Utilities;

public static class DtoGenerator
{
    public static Faker<DtoAuthentication> DtoAuthenticationFaker =
        new Faker<DtoAuthentication>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString())
            .RuleFor(o => o.Password, f => Guid.NewGuid().ToString());

    public static Faker<DtoSpacePost> DtoSpacePostFaker =
        new Faker<DtoSpacePost>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString());

}