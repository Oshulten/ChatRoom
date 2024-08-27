using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using static Backend.Tests.Utilities.DtoGenerator;
using Backend.Tests.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests.Api;

public class SpacesTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task PostSpaceReturns201Created()
    {
        var space = DtoSpacePostFaker.Generate(1)[0];

        var response = await SpacesEndpoints.PostSpaceResponse(_client, space);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task PostedSpaceCanBeFoundThroughGuid()
    {
        var space = DtoSpacePostFaker.Generate(1)[0];
        var postedSpace = await SpacesEndpoints.PostSpace(_client, space);

        postedSpace!.Guid.Should().NotBeEmpty();

        var response = await SpacesEndpoints.GetByGuidResponse(_client, postedSpace.Guid);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task<DtoSpace> PostEmptySpace()
    {
        var space = DtoSpacePostFaker.Generate(1)[0];
        return await SpacesEndpoints.PostSpace(_client, space);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task PostedSpaceExists()
    {
        var space = await PostEmptySpace();
        var response = await SpacesEndpoints.GetByGuidResponse(_client, space.Guid);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<DtoSpace>();

        content.Should().NotBeNull();
        content!.Guid.Should().NotBeEmpty();
        content.Alias.Should().Be(space.Alias);
    }

    // [Fact]
    // [Trait("Outcome", "Sad")]
    // public async Task AddMemberToNonExistingSpaceReturns404NotFound()
    // {
    //     var auth = DtoAuthenticationFaker.Generate(1)[0];
    //     var spaceGuid = Guid.NewGuid();
    //     var response = await SpacesEndpoints.AddExistingMemberToSpaceResponse(_client, spaceGuid, member)

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
    //     var response = await PutUserInSpace(space.Guid, user);

    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    // }

    // [Fact]
    // [Trait("Outcome", "Happy")]
    // public async Task AddingAUserToThreeSpacesShouldReturnThreeSpaces()
    // {
    //     var user = await PostUser();
    //     var space = await PostEmptySpace();
    //     var space1 = await PostEmptySpace();
    //     var space2 = await PostEmptySpace();

    //     await PutUserInSpace(space.Guid, user);
    //     await PutUserInSpace(space.Guid, user);
    //     await PutUserInSpace(space.Guid, user);

    //     GetSpaceByGuid()
    // }
}