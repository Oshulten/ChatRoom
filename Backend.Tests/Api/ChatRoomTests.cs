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

    public async Task<HttpResponseMessage> CreateUserByAuthResponse(DtoAuthentication auth) =>
        await _client.PostAsync("api/Chatroom", JsonContent.Create(auth));

    public async Task<DtoUser> CreateUserByAuth(DtoAuthentication auth)
    {
        var response = await CreateUserByAuthResponse(auth);
        return (await response.Content.ReadFromJsonAsync<DtoUser>())!;
    }

    [Fact]
    [Trait("Outcome", "Happy")]
    public async Task CreateNewUserByAuthShouldReturn201Created()
    {
        var auth = DtoAuthenticationFaker.Generate(1)[0];
        var response = await CreateUserByAuthResponse(auth);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    // [Fact]
    // [Trait("Outcome", "Sad")]
    // public async Task CreateUserWithExistingAliasByAuthShouldReturn400BadRequest()
    // {
    //     var auth = DtoAuthenticationFaker.Generate(1)[0];
    //     await CreateUserByAuth(auth);
    //     var response = await CreateUserByAuthResponse(auth);

    //     response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    // }

    // [Fact]
    // [Trait("Outcome", "Happy")]
    // public async Task CreateNewUserByAuthShouldReturnDtoUser()
    // {
    //     var auth = DtoAuthenticationFaker.Generate(1)[0];
    //     var dtoUser = await CreateUserByAuth(auth);

    //     dtoUser.Should().NotBeNull();
    //     dtoUser.Guid.Should().NotBeEmpty();
    //     dtoUser.Alias.Should().Be(auth.Alias);
    //     dtoUser.JoinedAt.Should().BeBefore(DateTime.Now);
    //     dtoUser.Admin.Should().BeFalse();
    // }

}