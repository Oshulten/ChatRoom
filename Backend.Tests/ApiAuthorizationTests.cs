using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests;

public class ApiAuthorizationTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private readonly Faker<DtoAuthentication> dtoAuthenticationFaker =
        new Faker<DtoAuthentication>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString())
            .RuleFor(o => o.Password, f => Guid.NewGuid().ToString());

    private readonly Faker<DtoSpacePost> dtoUserFaker =
        new Faker<DtoSpacePost>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString());

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserShouldReturn201Created()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserShouldReturnCorrectDto()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        var dto = await response.Content.ReadFromJsonAsync<DtoUser>();

        dto.Should().NotBeNull();
        dto!.Alias.Should().Be(auth.Alias);
        dto!.JoinedAt.Should().BeBefore(DateTime.Now);
        dto!.Admin.Should().BeFalse();
        dto!.Guid.Should().NotBeEmpty();
    }

    [Fact]
    [Trait("Outcome", "Sad")]
    public async Task CreateExistingUserShouldReturn400BadRequest()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        var response2 = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Outcome", "Sad")]
    public async Task AuthorizeNonExistingUserShouldReturn404NotFound()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsync("/api/Authentication/authorize-user", JsonContent.Create(auth));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task AuthorizeExistingUserShouldReturn200OK()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        var response = await _client.PostAsync("/api/Authentication/authorize-user", JsonContent.Create(auth));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task AuthorizeExistingUserShouldReturnCorrectDto()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        var response = await _client.PostAsync("/api/Authentication/authorize-user", JsonContent.Create(auth));

        var dto = await response.Content.ReadFromJsonAsync<DtoUser>();

        dto.Should().NotBeNull();
        dto!.Alias.Should().Be(auth.Alias);
        dto!.JoinedAt.Should().BeBefore(DateTime.Now);
        dto!.Admin.Should().BeFalse();
        dto!.Guid.Should().NotBeEmpty();
    }
}