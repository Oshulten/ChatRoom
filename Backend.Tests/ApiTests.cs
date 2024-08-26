using System.Net;
using System.Net.Http.Json;
using Backend.Dto;
using Backend.Models;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests;

public class ApiTests(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateUserShouldReturnDtoUser()
    {
        DtoAuthentication auth = new("Robert", "Doe");
        var response = await _client.PostAsync("/api/Authentication/create-user", JsonContent.Create(auth));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<DtoUser>();
        dto.Should().NotBeNull();
        dto!.Alias.Should().Be(auth.Alias);
    }
    // private async Task<GetCdResponse> PostValidCd()
    // {
    // var purchaseDate = DateTime.Now;
    // var cdRequest = new PostCdRequest("Blue on a monday", null, "A great album", purchaseDate, null);
    // var response = await _client.PostAsync("/api/CDs", JsonContent.Create(cdRequest));
    // return (await response.Content.ReadFromJsonAsync<GetCdResponse>())!;
    // }

    // private async Task<IEnumerable<GetCdResponse>> PostRandomValidCds(int postCount)
    // {
    //     var cdRequests = new Faker<Cd>()
    //         .RuleFor(o => o.ArtistName, f => f.Person.FullName)
    //         .RuleFor(o => o.Description, f => f.Rant.Review("album"))
    //         .RuleFor(o => o.Genre, f => new Genre(f.Music.Genre()))
    //         .RuleFor(o => o.PurchaseDate, f => DateTime.Now)
    //         .RuleFor(o => o.Name, f => f.Lorem.Word())
    //         .Generate(postCount)
    //         .Select(cd => (PostCdRequest)cd);

    //     var responses = cdRequests
    //         .Select(request => _client.PostAsync("/api/CDs", JsonContent.Create(request)));

    //     var contents = (await Task.WhenAll(responses))
    //         .Select(async response => (await response.Content.ReadFromJsonAsync<GetCdResponse>())!);

    //     return await Task.WhenAll(contents);
    // }

    // [Fact]
    // public async Task ShouldGetNotFoundForWrongURL()
    // {
    //     var response = await _client.GetAsync("/api");
    //     response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    // }

    // [Fact]
    // public async Task ShouldGetArrayFromGetAll()
    // {
    //     var response = await _client.GetAsync("/api/CDs");
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    //     var content = await response.Content.ReadFromJsonAsync<IEnumerable<GetCdResponse>>();

    //     content.Should().NotBeNull();
    //     content.Should().BeOfType<List<GetCdResponse>>();
    //     content!.Count().Should().BeGreaterThan(0);
    // }

    // [Fact]
    // public async Task PostCdShouldReturnCdResponse()
    // {
    //     var purchaseDate = DateTime.Now;
    //     var cdRequest = new PostCdRequest("Blue on a monday", null, "A great album", purchaseDate, null);

    //     var response = await _client.PostAsync("/api/CDs", JsonContent.Create(cdRequest));

    //     response.StatusCode.Should().Be(HttpStatusCode.Created);

    //     var content = await response.Content.ReadFromJsonAsync<GetCdResponse>();

    //     content.Should().NotBeNull();
    //     content!.Name.Should().Be(cdRequest.Name);
    //     content!.Description.Should().Be(cdRequest.Description);
    //     content!.ArtistName.Should().BeNull();
    //     content!.PurchaseDate.Should().Be(purchaseDate);
    //     content!.GenreName.Should().BeNull();
    // }

    // [Fact]
    // public async Task PostInvalidCdShouldReturnProblemDetails()
    // {
    //     var cdRequest = new PostCdRequest("", "", "", DateTime.Now, "");

    //     var response = await _client.PostAsync("/api/CDs", JsonContent.Create(cdRequest));

    //     response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    //     var content = (await response.Content.ReadFromJsonAsync<ProblemDetails>())!;

    //     content.Title.Should().Be("One or more validation errors occurred.");
    //     content.Status.Should().Be(400);
    // }

    // [Fact]
    // public async Task Posting5CdsShouldIncreaseCdCountBy5()
    // {
    //     var response = await _client.GetAsync("/api/CDs");
    //     var countBeforePosts = (await response.Content.ReadFromJsonAsync<IEnumerable<GetCdResponse>>())!.Count();

    //     await PostRandomValidCds(5);

    //     var secondResponse = await _client.GetAsync("/api/CDs");
    //     var countsAfterPosts = (await secondResponse.Content.ReadFromJsonAsync<IEnumerable<GetCdResponse>>())!.Count();

    //     (countsAfterPosts - countBeforePosts).Should().Be(5);
    // }

    // [Fact]
    // public async Task PostingCdWithGenreShouldIncreaseGenreCount()
    // {
    //     var beforeResponse = await _client.GetAsync("/api/CDs?genre=Jazz");
    //     var countBeforePosts = (await beforeResponse.Content.ReadFromJsonAsync<IEnumerable<GetCdResponse>>())!.Count();

    //     var postCdRequest = new PostCdRequest("Some albume", null, "An awful piece of music", DateTime.Now, "Jazz");
    //     await _client.PostAsync("/api/CDs", JsonContent.Create(postCdRequest));

    //     var afterResponse = await _client.GetAsync("/api/CDs?genre=Jazz");
    //     var countsAfterPosts = (await afterResponse.Content.ReadFromJsonAsync<IEnumerable<GetCdResponse>>())!.Count();

    //     (countsAfterPosts - countBeforePosts).Should().Be(1);
    // }

    // [Fact]
    // public async Task PuttingArtistNameShouldUpdateCd()
    // {
    //     var postCdRequest = new PostCdRequest("Some album", null, "An awful piece of music", DateTime.Now, null);
    //     var response = await _client.PostAsync("/api/CDs", JsonContent.Create(postCdRequest));
    //     var content = (await response.Content.ReadFromJsonAsync<GetCdResponse>())!;

    //     var cdId = content.Guid;
    //     content.ArtistName.Should().BeNull();

    //     var artistName = "Greatest musician ever";
    //     var putArtistRequest = new PutArtistRequest(artistName);
    //     var afterResponse = await _client.PutAsync($"/api/CDs/{cdId}/artist", JsonContent.Create(putArtistRequest));
    //     var afterContent = (await afterResponse.Content.ReadFromJsonAsync<GetCdResponse>())!;

    //     afterContent.ArtistName.Should().Be(artistName);
    // }

    // [Fact]
    // public async Task PuttingGenreShouldUpdateCd()
    // {
    //     var postCdRequest = new PostCdRequest("Some album", null, "An awful piece of music", DateTime.Now, null);
    //     var response = await _client.PostAsync("/api/CDs", JsonContent.Create(postCdRequest));
    //     var content = (await response.Content.ReadFromJsonAsync<GetCdResponse>())!;
    //     var cdId = content.Guid;

    //     content.GenreName.Should().BeNull();

    //     var genreName = "Psychedelic Post-Minimalist Retro-Techno";
    //     var putGenreRequest = new PutGenreRequest(genreName);
    //     var afterResponse = await _client.PutAsync($"/api/CDs/{cdId}/genre", JsonContent.Create(putGenreRequest));
    //     var afterContent = (await afterResponse.Content.ReadFromJsonAsync<GetCdResponse>())!;

    //     afterContent.GenreName.Should().Be(genreName);
    // }

    // [Fact]
    // public async Task PostCdShouldReturnLocationToCd()
    // {
    //     var postCdRequest = new PostCdRequest("Some album", null, "An awful piece of music", DateTime.Now, null);
    //     var response = await _client.PostAsync("/api/CDs", JsonContent.Create(postCdRequest));
    //     var content = (await response.Content.ReadFromJsonAsync<GetCdResponse>())!;
    //     var expectedUri = new Uri($"http://localhost/api/CDs/{content.Guid}");
    //     response.Headers.Location.Should().Be(expectedUri);
    // }

    // [Fact]
    // public async Task PostedCdShouldBeAccessibleAtLocation()
    // {
    //     var postCdRequest = new PostCdRequest("Some album", null, "An awful piece of music", DateTime.Now, null);
    //     var response = await _client.PostAsync("/api/CDs", JsonContent.Create(postCdRequest));
    //     var uri = response.Headers.Location;

    //     var getCdResponse = await _client.GetAsync(uri);
    //     var content = (await response.Content.ReadFromJsonAsync<GetCdResponse>())!;

    //     content.Should().NotBeNull();
    //     content.Guid.Should().NotBeEmpty();
    //     content.Name.Should().Be(postCdRequest.Name);
    //     content.ArtistName.Should().Be(postCdRequest.ArtistName);
    //     content.Description.Should().Be(postCdRequest.Description);
    //     content.PurchaseDate.Should().Be(postCdRequest.PurchaseDate);
    //     content.GenreName.Should().Be(postCdRequest.GenreName);
    // }
}