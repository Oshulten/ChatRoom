using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests;

public class ApiUsersTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
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
    [Trait("Outcome", "Saddy")]
    public async Task GetUserByIncorrectGuidShouldReturn404NotFound()
    {
        var response = await _client.GetAsync($"/api/Users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task GetUserByGuidShouldReturn200OK()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];

        var responseA = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        var user = await responseA.Content.ReadFromJsonAsync<DtoUser>();
        var responseB = await _client.GetAsync($"/api/Users/{user!.Guid}");

        responseB.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}