using System.Net.Http.Json;
using Backend.Dto;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests.Utilities;

public static class MessagesEndpoints
{
    public static async Task<HttpResponseMessage> GetBySpaceAndDateResponse(HttpClient client, Guid spaceGuid, DateTime? messagesBefore, int? numberOfMessages) =>
        await client.GetAsync($"/api/Messages/{spaceGuid}?messagesBefore={messagesBefore}&numberOfMessages={numberOfMessages}");

    public static async Task<DtoMessageSequence> GetBySpaceAndDate(HttpClient client, Guid spaceGuid, DateTime? messagesBefore, int? numberOfMessages)
    {
        var response = await GetBySpaceAndDateResponse(client, spaceGuid, messagesBefore, numberOfMessages);
        return (await response.Content.ReadFromJsonAsync<DtoMessageSequence>())!;
    }

    public static async Task<HttpResponseMessage> PostMessageResponse(HttpClient client, DtoMessagePost post) =>
        await client.PostAsync($"/api/Messages", JsonContent.Create(post));

    public static async Task<DtoMessage> PostMessage(HttpClient client, DtoMessagePost post)
    {
        var response = await PostMessageResponse(client, post);
        return (await response.Content.ReadFromJsonAsync<DtoMessage>())!;
    }
}