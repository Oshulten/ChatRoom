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

        var auth = new DtoAuthentication("user1", "password1");
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Sad")]
    public async Task CreateUserWithExistingAliasByAuthShouldReturn400BadRequest()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = new DtoAuthentication("user2", "password2");
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

        var auth = new DtoAuthentication("user3", "password3");
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

        var auth = new DtoAuthentication("user4", "password4");
        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Happy")]
    public async Task GetExistingUserByAuthShouldReturn200Ok()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = new DtoAuthentication("user5", "password5");
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

        var auth = new DtoAuthentication("user6", "password6");
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

        var dtoSpacePost = new DtoSpacePost("space1");
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-space")]
    [Trait("Outcome", "Happy")]
    public async Task CreateSpaceShouldReturnDtoSpace()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoSpacePost = new DtoSpacePost("space2");
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

        var auth = new DtoAuthentication("user7", "password7");
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
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = new DtoAuthentication("user8", "password8");
        var responseA = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var dtoUser = await responseA.Content.ReadFromJsonAsync<DtoUser>();

        var dtoSpacePost = new DtoSpacePost("space2");
        var responseB = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await responseB.Content.ReadFromJsonAsync<DtoSpace>();

        var responseC = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", dtoUser);

        responseC.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task AddExistingUserToExistingSpaceShouldReturnDtoSpace()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = new DtoAuthentication("user8", "password8");
        var responseA = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        responseA.StatusCode.Should().Be(HttpStatusCode.Created);

        var dtoUser = await responseA.Content.ReadFromJsonAsync<DtoUser>();

        var dtoSpacePost = new DtoSpacePost("space2");
        var responseB = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);

        responseB.StatusCode.Should().Be(HttpStatusCode.Created);

        var dtoSpace = await responseB.Content.ReadFromJsonAsync<DtoSpace>();

        var responseC = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", dtoUser);

        responseC.StatusCode.Should().Be(HttpStatusCode.OK);

        var dtoSpace2 = await responseC.Content.ReadFromJsonAsync<DtoSpace>();

        var countDifference = (dtoSpace2!.MemberGuids.Count() - dtoSpace!.MemberGuids.Count());

        dtoSpace2.Should().NotBeNull();
        dtoSpace2!.Alias.Should().Be(dtoSpacePost.Alias);
        dtoSpace2!.Guid.Should().NotBeEmpty();
        countDifference.Should().Be(1);
    }

    // [Fact]
    // [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    // [Trait("Outcome", "Happy")]
    // public async Task AddTwoExistingUsersToExistingSpaceShouldReturnDtoSpaceWithTwoMembers()
    // {
    //     var auth = DtoAuthenticationFaker.Generate(1)[0];
    //     await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
    //     var auth2 = DtoAuthenticationFaker.Generate(1)[0];
    //     await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth2);

    //     var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
    //     var responseB = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
    //     var dtoSpace = await responseB.Content.ReadFromJsonAsync<DtoSpace>();

    //     dtoSpace.Should().NotBeNull();
    //     dtoSpace!.MemberGuids.Count.Should().Be(2);
    // }

    // [Fact]
    // [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    // [Trait("Outcome", "Sad")]
    // public async Task AddNonExistingUserToExistingSpaceShouldReturn404NotFound()
    // {
    //     var nonExistingDtoUser = new DtoUser(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.Now, false);

    //     var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
    //     var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
    //     var dtoSpace = await response.Content.ReadFromJsonAsync<DtoSpace>();

    //     var responseB = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", nonExistingDtoUser);

    //     responseB.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    // }
}