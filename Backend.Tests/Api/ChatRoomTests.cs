using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using static Backend.Tests.Utilities.DtoGenerator;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests.Api;

public class ChatRoomTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserByAuthShouldReturn201Created()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Sad")]
    public async Task CreateUserWithExistingAliasByAuthShouldReturn400BadRequest()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserByAuthShouldReturnDtoUser()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var dtoUser = await response.Content.ReadFromJsonAsync<DtoUser>();

        dtoUser.Should().NotBeNull();
        dtoUser!.Guid.Should().NotBeEmpty();
        dtoUser.Alias.Should().Be(auth.Alias);
        dtoUser.JoinedAt.Should().BeBefore(DateTime.Now);
        dtoUser.Admin.Should().BeFalse();
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Sad")]
    public async Task GetNotExistingUserByAuthShouldReturn400BadRequest()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Happy")]
    public async Task GetExistingUserByAuthShouldReturn200Ok()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Happy")]
    public async Task GetExistingUserByAuthShouldReturnDtoUser()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);
        var dtoUser = await response.Content.ReadFromJsonAsync<DtoUser>();

        dtoUser.Should().NotBeNull();
        dtoUser!.Guid.Should().NotBeEmpty();
        dtoUser.Alias.Should().Be(auth.Alias);
        dtoUser.JoinedAt.Should().BeBefore(DateTime.Now);
        dtoUser.Admin.Should().BeFalse();
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-space")]
    [Trait("Outcome", "Happy")]
    public async Task CreateSpaceShouldReturn201Created()
    {
        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-space")]
    [Trait("Outcome", "Happy")]
    public async Task CreateSpaceShouldReturnDtoSpace()
    {
        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await response.Content.ReadFromJsonAsync<DtoSpace>();

        dtoSpace.Should().NotBeNull();
        dtoSpace!.Guid.Should().NotBeEmpty();
        dtoSpace.Alias.Should().Be(dtoSpacePost.Alias);
        dtoSpace.MemberGuids.Should().BeEmpty();
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Sad")]
    public async Task AddExistingUserToNonExistingSpaceShouldReturn404NotFound()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var responseA = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var dtoUser = await responseA.Content.ReadFromJsonAsync<DtoUser>();

        var nonExistingSpaceGuid = Guid.NewGuid();

        var responseB = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{nonExistingSpaceGuid}", dtoUser);

        responseB.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddExistingUserToExistingSpaceShouldReturn200Ok()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var responseA = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var dtoUser = await responseA.Content.ReadFromJsonAsync<DtoUser>();

        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var responseB = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);

        responseB.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddExistingUserToExistingSpaceShouldReturnDtoSpace()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var responseA = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var responseB = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await responseB.Content.ReadFromJsonAsync<DtoSpace>();

        dtoSpace.Should().NotBeNull();
        dtoSpace!.Alias.Should().Be(dtoSpacePost.Alias);
        dtoSpace!.Guid.Should().NotBeEmpty();
        dtoSpace!.MemberGuids.Count.Should().Be(1);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddTwoExistingUsersToExistingSpaceShouldReturnDtoSpaceWithTwoMembers()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var auth2 = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth2);

        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var responseB = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await responseB.Content.ReadFromJsonAsync<DtoSpace>();

        dtoSpace.Should().NotBeNull();
        dtoSpace!.MemberGuids.Count.Should().Be(2);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Sad")]
    public async Task AddNonExistingUserToExistingSpaceShouldReturn404NotFound()
    {
        var nonExistingDtoUser = new DtoUser(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.Now, false);

        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await response.Content.ReadFromJsonAsync<DtoSpace>();

        var responseB = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", nonExistingDtoUser);

        responseB.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}