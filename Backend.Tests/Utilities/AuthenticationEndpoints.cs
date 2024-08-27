using System.Net.Http.Json;
using Backend.Dto;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests.Utilities;

public static class AuthenticationEndpoints
{
    public static async Task<HttpResponseMessage> CreateUserResponse(HttpClient client, DtoAuthentication auth) =>
        await client.PostAsync($"/api/Authentication/create-user", JsonContent.Create(auth));

    public static async Task<DtoUser> CreateUser(HttpClient client, DtoAuthentication auth)
    {
        var response = await CreateUserResponse(client, auth);
        return (await response.Content.ReadFromJsonAsync<DtoUser>())!;
    }

    public static async Task<HttpResponseMessage> AuthorizeUserResponse(HttpClient client, DtoAuthentication auth) =>
        await client.PostAsync($"/api/Authentication/authorize-user", JsonContent.Create(auth));

    public static async Task<DtoUser> AuthorizeUser(HttpClient client, DtoAuthentication auth)
    {
        var response = await AuthorizeUserResponse(client, auth);
        return (await response.Content.ReadFromJsonAsync<DtoUser>())!;
    }
}