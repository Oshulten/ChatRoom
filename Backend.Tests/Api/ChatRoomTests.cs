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

    private async Task<DtoUser> PostUser()
    {
        var auth = GenerateDtoAuthentication();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        return (await response.Content.ReadFromJsonAsync<DtoUser>())!;
    }

    private async Task<DtoSpace> PostSpace()
    {
        var dtoSpacePost = GenerateDtoSpacePost();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        return (await response.Content.ReadFromJsonAsync<DtoSpace>())!;
    }

    private async Task<DtoSpace> AddUserToSpace(DtoUser user, DtoSpace space)
    {
        var response = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{space!.Guid}", user);
        return (await response.Content.ReadFromJsonAsync<DtoSpace>())!;
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/clear")]
    [Trait("Outcome", "Happy")]
    public async Task ClearShouldRemoveUsersSpacesMessages()
    {
        var responseClear = await _client.PostAsync("api/Chatroom/clear", null);
        responseClear.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseUsers = await _client.GetAsync("api/Chatroom/get-users");
        responseUsers.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await responseUsers.Content.ReadFromJsonAsync<IEnumerable<DtoUser>>();
        users!.Count().Should().Be(0);

        var responseSpaces = await _client.GetAsync("api/Chatroom/get-spaces");
        responseSpaces.StatusCode.Should().Be(HttpStatusCode.OK);
        var spaces = await responseSpaces.Content.ReadFromJsonAsync<IEnumerable<DtoSpace>>();
        spaces!.Count().Should().Be(0);

        var responseMessages = await _client.GetAsync("api/Chatroom/get-messages");
        responseMessages.StatusCode.Should().Be(HttpStatusCode.OK);
        var messages = await responseMessages.Content.ReadFromJsonAsync<IEnumerable<DtoMessage>>();
        messages!.Count().Should().Be(0);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserByAuthShouldReturn201Created()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Sad")]
    public async Task CreateUserWithExistingAliasByAuthShouldReturn400BadRequest()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserByAuthShouldReturnDtoUser()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
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
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Happy")]
    public async Task GetExistingUserByAuthShouldReturn200Ok()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Happy")]
    public async Task GetExistingUserByAuthShouldReturnDtoUser()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
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
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoSpacePost = GenerateDtoSpacePost();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-space")]
    [Trait("Outcome", "Happy")]
    public async Task CreateSpaceShouldReturnDtoSpace()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoSpacePost = GenerateDtoSpacePost();
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
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser = await PostUser();

        var nonExistingSpaceGuid = Guid.NewGuid();

        var responseB = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{nonExistingSpaceGuid}", dtoUser);

        responseB.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddExistingUserToExistingSpaceShouldReturn200Ok()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser = await PostUser();
        var dtoSpace = await PostSpace();

        var response = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", dtoUser);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(Skip = "Unknown issue")]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddExistingUserToExistingSpaceShouldReturnDtoSpace()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser = await PostUser();
        var dtoSpace = await PostSpace();
        var dtoSpace2 = await AddUserToSpace(dtoUser, dtoSpace);

        var countDifference = (dtoSpace2!.MemberGuids.Count() - dtoSpace!.MemberGuids.Count());

        dtoSpace2.Should().NotBeNull();
        dtoSpace2!.Alias.Should().Be(dtoSpace.Alias);
        dtoSpace2!.Guid.Should().NotBeEmpty();
        countDifference.Should().Be(1);
    }

    [Fact(Skip = "Unknown issue")]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddTwoExistingUsersToExistingSpaceShouldReturnDtoSpaceWithTwoMembers()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser1 = await PostUser();
        var dtoUser2 = await PostUser();
        var dtoSpace = await PostSpace();
        dtoSpace = await AddUserToSpace(dtoUser1, dtoSpace);
        dtoSpace = await AddUserToSpace(dtoUser2, dtoSpace);

        dtoSpace.Should().NotBeNull();
        dtoSpace!.MemberGuids.Count.Should().Be(2);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Sad")]
    public async Task AddNonExistingUserToExistingSpaceShouldReturn404NotFound()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var nonExistingDtoUser = new DtoUser(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.Now, false);

        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await response.Content.ReadFromJsonAsync<DtoSpace>();

        var responseB = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", nonExistingDtoUser);

        responseB.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


}