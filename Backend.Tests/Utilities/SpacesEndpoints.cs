using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Dto;
using Microsoft.AspNetCore.Http;
using static Backend.Tests.Utilities.DtoGenerator;

namespace Backend.Tests.Utilities;

public static class SpacesEndpoints
{
    public static async Task<HttpResponseMessage> GetByUserGuidResponse(HttpClient client, Guid userGuid) =>
        await client.GetAsync($"/api/Spaces/{userGuid}");

    public static async Task<IEnumerable<DtoSpace>> GetByUserGuid(HttpClient client, Guid userGuid)
    {
        var response = await GetByUserGuidResponse(client, userGuid);
        return (await response.Content.ReadFromJsonAsync<IEnumerable<DtoSpace>>())!;
    }

    public static async Task<HttpResponseMessage> GetByGuidResponse(HttpClient client, Guid spaceGuid) =>
        await client.GetAsync($"/api/Spaces/{spaceGuid}");

    public static async Task<DtoSpace> GetByGuid(HttpClient client, Guid spaceGuid)
    {
        var response = await GetByGuidResponse(client, spaceGuid);
        return (await response.Content.ReadFromJsonAsync<DtoSpace>())!;
    }

    public static async Task<HttpResponseMessage> PostSpaceResponse(HttpClient client, DtoSpacePost space) =>
        await client.PostAsync($"/api/Spaces", JsonContent.Create(space));

    public static async Task<DtoSpace> PostSpace(HttpClient client, DtoSpacePost space)
    {
        var response = await PostSpaceResponse(client, space);
        return (await response.Content.ReadFromJsonAsync<DtoSpace>())!;
    }

    public static async Task<HttpResponseMessage> AddExistingMemberToSpaceResponse(HttpClient client, Guid spaceGuid, DtoUser user) =>
        await client.PostAsync($"/api/Spaces/{spaceGuid}", JsonContent.Create(user));
}