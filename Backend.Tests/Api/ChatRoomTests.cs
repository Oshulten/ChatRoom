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
        var response = await _client.PutAsync($"api/Chatroom/add-user-to-space/{space!.Guid}?userGuid={user.Guid}", null);
        return (await response.Content.ReadFromJsonAsync<DtoSpace>())!;
    }

    private async Task<DtoMessage> PostMessage(DtoSpace space, DtoUser sender)
    {
        var dtoMessagePost = new DtoMessage("Message content", space.Guid, sender.Guid, DateTime.Now);
        var response = await _client.PostAsJsonAsync("api/Chatroom/create-message", dtoMessagePost);
        return (await response.Content.ReadFromJsonAsync<DtoMessage>())!;
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/clear")]
    [Trait("Outcome", "Happy")]
    public async Task Clear_Should_Remove_Users_Spaces_Messages()
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
    public async Task Create_New_User_By_Auth_Should_Return_201_Created()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-user", auth);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-user")]
    [Trait("Outcome", "Sad")]
    public async Task Create_User_With_Existing_Alias_By_Auth_Should_Return_400_Bad_Request()
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
    public async Task Create_New_User_By_Auth_Should_Return_Dto_User()
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
    public async Task Get_Not_Existing_User_By_Auth_Should_Return_400_Bad_Request()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var auth = GenerateDtoAuthentication();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/get-user-by-auth", auth);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-user-by-auth")]
    [Trait("Outcome", "Happy")]
    public async Task Get_Existing_User_By_Auth_Should_Return_200_OK()
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
    public async Task Get_Existing_User_By_Auth_Should_Return_Dto_User()
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
    public async Task Create_Space_Should_Return_201_Created()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoSpacePost = GenerateDtoSpacePost();
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-space")]
    [Trait("Outcome", "Happy")]
    public async Task Create_Space_Should_Return_Dto_Space()
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
    public async Task Add_Existing_User_To_Non_Existing_Space_Should_Return_404_Not_Found()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser = await PostUser();

        var nonExistingSpaceGuid = Guid.NewGuid();

        var responseB = await _client.PutAsync($"api/Chatroom/add-user-to-space/{nonExistingSpaceGuid}?userGuid={dtoUser.Guid}", null);

        responseB.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task Add_Existing_User_To_Existing_Space_Should_Return_200_OK()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser = await PostUser();
        var dtoSpace = await PostSpace();

        var response = await _client.PutAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}?userGuid={dtoUser.Guid}", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task Add_Existing_User_To_Existing_Space_Should_Return_Dto_Space()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser = await PostUser();
        var dtoSpace = await PostSpace();
        var dtoSpace2 = await AddUserToSpace(dtoUser, dtoSpace);

        var countDifference = dtoSpace2!.MemberGuids.Count - dtoSpace!.MemberGuids.Count;

        dtoSpace2.Should().NotBeNull();
        dtoSpace2!.Alias.Should().Be(dtoSpace.Alias);
        dtoSpace2!.Guid.Should().NotBeEmpty();
        countDifference.Should().Be(1);
    }

    [Fact(Skip = "Unknown issue")]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Happy")]
    public async Task Add_Two_Existing_Users_To_Existing_Space_Should_Return_Dto_Space_With_Two_Members()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoUser1 = await PostUser();
        var dtoUser2 = await PostUser();
        var dtoSpace = await PostSpace();

        dtoSpace = await AddUserToSpace(dtoUser1, dtoSpace);
        dtoSpace = await AddUserToSpace(dtoUser2, dtoSpace);

        dtoSpace.Should().NotBeNull();
        dtoSpace.MemberGuids.Where(guid => guid == dtoUser1.Guid).Should().NotBeEmpty();
        dtoSpace.MemberGuids.Where(guid => guid == dtoUser2.Guid).Should().NotBeEmpty();
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/add-user-to-space")]
    [Trait("Outcome", "Sad")]
    public async Task Add_Non_Existing_User_To_Existing_Space_Should_Return_404_Not_Found()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var nonExistingDtoUser = new DtoUser(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.Now, false);

        var dtoSpacePost = new DtoSpacePost(Guid.NewGuid().ToString());
        var response = await _client.PostAsJsonAsync($"api/Chatroom/create-space", dtoSpacePost);
        var dtoSpace = await response.Content.ReadFromJsonAsync<DtoSpace>();

        var responseB = await _client.PutAsJsonAsync($"api/Chatroom/add-user-to-space/{dtoSpace!.Guid}", nonExistingDtoUser);

        responseB.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-spaces-by-user-guid")]
    [Trait("Outcome", "Sad")]
    public async Task Get_Space_By_User_Guid_For_Non_Existing_User_Should_Return_404_Not_Found()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var response = await _client.GetAsync($"api/Chatroom/get-spaces-by-user-guid/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-spaces-by-user-guid")]
    [Trait("Outcome", "Happy")]
    public async Task Get_Space_By_User_Guid_Should_Return_Empty_List_For_User_Not_Belonging_To_Any_Space()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var user = await PostUser();

        var response = await _client.GetAsync($"api/Chatroom/get-spaces-by-user-guid/{user.Guid}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var spaces = await response.Content.ReadFromJsonAsync<List<DtoSpace>>();

        spaces!.Count.Should().Be(0);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-spaces-by-user-guid")]
    [Trait("Outcome", "Happy")]
    public async Task Get_Space_By_User_Guid_Should_Return_List_Of_One_For_User_Added_To_One_Space()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var user = await PostUser();
        var space = await PostSpace();
        await AddUserToSpace(user, space);

        var response = await _client.GetAsync($"api/Chatroom/get-spaces-by-user-guid/{user.Guid}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var spaces = await response.Content.ReadFromJsonAsync<List<DtoSpace>>();

        spaces!.FirstOrDefault(s => s.Guid == space.Guid).Should().NotBeNull();
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-spaces-by-user-guid")]
    [Trait("Outcome", "Happy")]
    public async Task Get_Space_By_User_Guid_Should_Return_List_Of_Two_For_User_Added_To_Two_Spaces()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var user = await PostUser();
        var space1 = await PostSpace();
        var space2 = await PostSpace();

        await AddUserToSpace(user, space1);
        await AddUserToSpace(user, space2);

        var response = await _client.GetAsync($"api/Chatroom/get-spaces-by-user-guid/{user.Guid}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var spaces = await response.Content.ReadFromJsonAsync<List<DtoSpace>>();

        spaces!.FirstOrDefault(s => s.Guid == space1.Guid).Should().NotBeNull();
        spaces!.FirstOrDefault(s => s.Guid == space2.Guid).Should().NotBeNull();
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-message")]
    [Trait("Outcome", "Sad")]
    public async Task Create_Message_From_Non_Existing_Sender_Should_Return_400_Bad_Request()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoMessagePost = new DtoMessage("Message content", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
        var response = await _client.PostAsJsonAsync("api/Chatroom/create-message", dtoMessagePost);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-message")]
    [Trait("Outcome", "Sad")]
    public async Task Create_Message_In_Non_Existing_Space_Should_Return_400_Bad_Request()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoMessagePost = new DtoMessage("Message content", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
        var response = await _client.PostAsJsonAsync("api/Chatroom/create-message", dtoMessagePost);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-message")]
    [Trait("Outcome", "Happy")]
    public async Task Create_Valid_Message_Should_Return_201_Created()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoSpace = await PostSpace();
        var dtoUser = await PostUser();

        var dtoMessagePost = new DtoMessage("Message content", dtoSpace.Guid, dtoUser.Guid, DateTime.Now);
        var response = await _client.PostAsJsonAsync("api/Chatroom/create-message", dtoMessagePost);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/create-message")]
    [Trait("Outcome", "Happy")]
    public async Task Create_Valid_Message_Should_Return_Dto_Message()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var dtoSpace = await PostSpace();
        var dtoUser = await PostUser();

        var dtoMessagePost = new DtoMessage("Message content", dtoSpace.Guid, dtoUser.Guid, DateTime.Now);
        var response = await _client.PostAsJsonAsync("api/Chatroom/create-message", dtoMessagePost);
        var dtoMessage = await response.Content.ReadFromJsonAsync<DtoMessage>();

        dtoMessage!.Content.Should().Be("Message content");
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-messages-in-space")]
    [Trait("Outcome", "Sad")]
    public async Task Get_Messages_In_Non_Existing_Space_Should_Return_400_Bad_Request()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var response = await _client.GetAsync($"api/Chatroom/get-messages-in-space/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-messages-in-space")]
    [Trait("Outcome", "Happy")]
    public async Task Get_Messages_In_Existing_Space_Should_Return_200_OK()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var space = await PostSpace();
        var user = await PostUser();
        await PostMessage(space, user);

        var response = await _client.GetAsync($"api/Chatroom/get-messages-in-space/{space.Guid}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Endpoint", "api/Chatroom/get-messages-in-space")]
    [Trait("Outcome", "Happy")]
    public async Task Posting_Messages_In_Space_Should_Return_List_Of_Messages()
    {
        await _client.PostAsync("api/Chatroom/clear", null);

        var space = await PostSpace();
        var user = await PostUser();
        await PostMessage(space, user);
        await PostMessage(space, user);
        await PostMessage(space, user);

        var response = await _client.GetAsync($"api/Chatroom/get-messages-in-space/{space.Guid}");
        var messages = await response.Content.ReadFromJsonAsync<List<DtoMessage>>();

        messages.Should().NotBeEmpty();
    }
}