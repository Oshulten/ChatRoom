using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using static Backend.Tests.Utilities.DtoGenerator;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests.Api;

public class MessagesTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

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