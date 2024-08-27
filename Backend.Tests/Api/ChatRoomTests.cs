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
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserByAuthShouldReturn201Created()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Outcome", "Sad")]
    public async Task CreateUserWithExistingAliasByAuthShouldReturn400BadRequest()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
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
    [Trait("Outcome", "Sad")]
    public async Task GetNotExistingUserByAuthShouldReturn400BadRequest()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task GetExistingUserByAuthShouldReturn200Ok()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
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

}