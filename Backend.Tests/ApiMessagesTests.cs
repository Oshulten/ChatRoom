using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests;

public class ApiMessagesTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private readonly Faker<DtoAuthentication> dtoAuthenticationFaker =
        new Faker<DtoAuthentication>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString())
            .RuleFor(o => o.Password, f => Guid.NewGuid().ToString());

    private readonly Faker<DtoSpacePost> dtoUserFaker =
        new Faker<DtoSpacePost>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString());

    [Fact(Skip = nameof(CreateSpaceShouldReturnDtoSpace))]
    public async Task CreateSpaceShouldReturnDtoSpace()
    {
        var space = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsync("/api/Spaces", JsonContent.Create(space));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var dto = await response.Content.ReadFromJsonAsync<DtoSpace>();

        dto.Should().NotBeNull();
        dto!.MemberGuids.Should().BeEmpty();
        dto!.Alias.Should().Be(space.Alias);
    }

    [Fact(Skip = nameof(GetMessagesBySpaceAndDateShouldReturnAllAddedMessages))]
    public async Task GetMessagesBySpaceAndDateShouldReturnAllAddedMessages()
    {
        var space = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsync("/api/Spaces", JsonContent.Create(space));
        var spaceDto = await response.Content.ReadFromJsonAsync<DtoSpace>();


    }
}