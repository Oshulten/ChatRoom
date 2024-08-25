using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(ChatroomDatabaseContext db) : ControllerBase
    {
        [HttpPost("create-user")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DtoUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DtoUser> CreateUser(DtoAuthentication request)
        {
            var existingUser = db.Users.FirstOrDefault(user => user.Alias == request.Alias);

            if (existingUser is not null)
            {
                return BadRequest();
            }

            var newUser = (DbUser)request;
            db.Users.Add(newUser);
            db.SaveChanges();
            return (DtoUser)newUser;
        }

        [HttpPost("authorize-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoUser> AuthorizeUser(DtoAuthentication request)
        {
            var existingUser = db.Users.FirstOrDefault(user => user.Alias == request.Alias && user.Password == request.Password);

            if (existingUser is null)
            {
                return NotFound();
            }

            return (DtoUser)existingUser;
        }
    }
}