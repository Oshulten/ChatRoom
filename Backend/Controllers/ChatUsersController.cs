using Backend.Models;
using Backend.Models.ChatUser;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatUsersController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet("{id}")]
    public ChatUserResponse GetById(Guid id)
    {
        return (ChatUserResponse)db.ChatUsers.FirstOrDefault(data => data.Id == id)!;
    }

    [HttpGet]
    public List<ChatUserResponse> GetAll()
    {
        return db.ChatUsers.Select(user => (ChatUserResponse)user).ToList()!;
    }

    [HttpPost("authenticate")]
    public ChatUserResponse Login(LoginRequest loginRequest)
    {
        var user = db.ChatUsers.FirstOrDefault(user =>
            user.Alias == loginRequest.Username &&
            user.Password == loginRequest.Password);
        if (user is not null) return (ChatUserResponse)user;
        return null!;
    }

    [HttpPost("create-account")]
    public ChatUserResponse CreateAccount(LoginRequest loginRequest)
    {
        var user = db.ChatUsers.FirstOrDefault(user => user.Alias == loginRequest.Username);
        if (user is null)
        {
            var newUser = new ChatUser(loginRequest.Username, loginRequest.Password, false, DateTime.Now);
            db.ChatUsers.Add(newUser);
            db.SaveChanges();
            return (ChatUserResponse)newUser;
        }
        return null!;
    }

}