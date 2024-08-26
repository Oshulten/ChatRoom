using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests;

public class ApiSpacesTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private readonly Faker<DtoAuthentication> dtoAuthenticationFaker =
        new Faker<DtoAuthentication>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString())
            .RuleFor(o => o.Password, f => Guid.NewGuid().ToString());

    private readonly Faker<DtoSpacePost> dtoUserFaker =
        new Faker<DtoSpacePost>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString());

    private readonly Faker<DtoSpacePost> dtoSpacePostFaker =
        new Faker<DtoSpacePost>()
            .RuleFor(o => o.Alias, f => Guid.NewGuid().ToString());

    private async Task<DtoUser> PostUser()
    {
        var auth = dtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        return (await response.Content.ReadFromJsonAsync<DtoUser>())!;
    }

    private async Task<DtoSpace> PostEmptySpace()
    {
        var space = dtoSpacePostFaker.Generate(1)[0];
        var response = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(space));
        return (await response.Content.ReadFromJsonAsync<DtoSpace>())!;
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task PostSpaceReturns201Created()
    {
        var space = dtoSpacePostFaker.Generate(1)[0];

        var response = await _client.PostAsync($"/api/Spaces", JsonContent.Create(space));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task PostedSpaceCanBeFoundThroughGuid()
    {
        var space = dtoSpacePostFaker.Generate(1)[0];

        var responseA = await _client.PostAsync($"/api/Spaces", JsonContent.Create(space));
        var postedSpace = await responseA.Content.ReadFromJsonAsync<DtoSpace>();

        postedSpace!.Guid.Should().NotBeEmpty();

        var responseB = await _client.GetAsync($"/api/Spaces/{postedSpace.Guid.ToString()}");

        responseB.StatusCode.Should().Be(HttpStatusCode.OK);

    }


    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task PostedSpaceExists()
    {
        var space = await PostEmptySpace();
        var response = await _client.GetAsync($"/api/Spaces/{space.Guid}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<DtoSpace>();


        content.Should().NotBeNull();
        content!.Guid.Should().NotBeEmpty();
        content.Alias.Should().Be(space.Alias);
        content.MemberGuids.Should().BeEmpty();
    }

    // [Fact]
    // [Trait("Outcome", "Sad")]
    // public async Task AddMemberToNonExistingSpaceReturns404NotFound()
    // {
    //     var dto = dtoSpacePostFaker.Generate(1)[0];
    //     var spaceGuid = Guid.NewGuid();
    //     var response = await _client.PutAsync($"/api/Spaces/{spaceGuid}", JsonContent.Create(dto));

    //     response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    // }

    // [Fact]
    // [Trait("Outcome", "Sad")]
    // public async Task AddNonExistingMemberToExistingSpaceReturns404NotFound()
    // {
    //     var space = await PostEmptySpace();
    //     var user = dtoUserFaker.Generate(1)[0];

    //     var response = await _client.PutAsync($"/api/Spaces/{space.Guid}", JsonContent.Create(user));

    //     response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    // }

    // [Fact]
    // [Trait("Outcome", "Happy")]
    // public async Task AddExistingMemberToExistingSpaceReturns200OK()
    // {
    //     var user = await PostUser();
    //     var space = await PostEmptySpace();
    //     var response = await _client.PutAsync($"/api/Spaces/{space.Guid}", JsonContent.Create(user));

    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    // }

    // [Fact]
    // [Trait("Outcome", "Sad")]
    // public async Task GetSpacesBySpacelessUserReturnsEmptyList()
    // {
    //     var user = await PostUser();
    //     var space = await PostEmptySpace();
    //     var response = await _client.PutAsync($"/api/Spaces/{space.Guid}", JsonContent.Create(user));

    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    // }
}