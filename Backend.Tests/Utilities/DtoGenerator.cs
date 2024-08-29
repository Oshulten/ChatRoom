using Backend.Dto;

namespace Backend.Tests.Utilities;

public static class DtoGenerator
{
    private static readonly Random _random = new(123);

    public static DtoAuthentication GenerateDtoAuthentication() =>
        new(_random.NextDouble().ToString(), _random.NextDouble().ToString());

    public static DtoSpacePost GenerateDtoSpacePost() =>
        new(_random.NextDouble().ToString());
}