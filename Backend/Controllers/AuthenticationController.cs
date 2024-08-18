using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Models.ChatUser;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpPost("create-user")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChatUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ChatUserResponse> CreateUser(LoginRequest request)
        {
            Console.WriteLine($"Requested Username: {request.Username}");
            var existingUser = db.ChatUsers.FirstOrDefault(user => user.Alias == request.Username);
            Console.WriteLine(existingUser);
            if (existingUser is not null)
            {
                return BadRequest();
            }
            var newUser = (ChatUser)request;
            db.ChatUsers.Add(newUser);
            db.SaveChanges();
            return (ChatUserResponse)newUser;
        }

        [HttpPost("authorize-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatUserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChatUserResponse> AuthorizeUser(LoginRequest request)
        {
            var existingUser = db.ChatUsers.FirstOrDefault(user => user.Alias == request.Username && user.Password == request.Password);
            if (existingUser is null)
            {
                return NotFound();
            }
            return (ChatUserResponse)existingUser;
        }
    }
}