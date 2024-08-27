using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Dto;
using Microsoft.AspNetCore.Http;
using static Backend.Tests.Utilities.DtoGenerator;

namespace Backend.Tests.Utilities;

public static class UsersEndpoints
{
    public static async Task<HttpResponseMessage> GetByUserGuidResponse(HttpClient client, Guid userGuid) =>
        await client.GetAsync($"/api/Users/{userGuid}");

    public static async Task<DtoUser> GetByUserGuid(HttpClient client, Guid userGuid)
    {
        var response = await GetByUserGuidResponse(client, userGuid);
        return (await response.Content.ReadFromJsonAsync<DtoUser>())!;
    }
}