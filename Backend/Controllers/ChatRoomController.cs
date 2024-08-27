using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatroomController(ChatroomDatabaseContext context) : ControllerBase
    {
        //Tested (3)
        [HttpPost("create-user")]
        [ProducesResponseType(201, Type = typeof(DtoUser))]
        [ProducesResponseType(400)]
        public ActionResult<DtoUser> CreateUserByAuth(DtoAuthentication auth)
        {
            var existingUser = context.Users.FirstOrDefault(user => user.Alias == auth.Alias);

            if (existingUser is null)
            {
                var user = new User(auth.Alias, auth.Password, false, DateTime.Now);
                context.Users.Add(user);
                context.SaveChanges();

                var dtoUser = new DtoUser(user.Guid, user.Alias, user.JoinedAt, user.Admin);
                return CreatedAtAction(null, dtoUser);
            }

            return BadRequest($"A user with alias {auth.Alias} already exists.");
        }

        //Tested (3)
        [HttpPost("get-user-by-auth")]
        [ProducesResponseType(200, Type = typeof(DtoUser))]
        [ProducesResponseType(400)]
        public ActionResult<DtoUser> GetUserByAuth(DtoAuthentication auth)
        {
            var matchingUser = context.Users.FirstOrDefault(user =>
                user.Alias == auth.Alias &&
                user.Password == auth.Password);

            if (matchingUser is null)
            {
                return BadRequest($"A user with that alias and password does not exist");
            }

            var dtoUser = new DtoUser(matchingUser.Guid, matchingUser.Alias, matchingUser.JoinedAt, matchingUser.Admin);
            return Ok(dtoUser);
        }

        //Tested (2)
        [HttpPost("create-space")]
        [ProducesResponseType(201, Type = typeof(DtoSpace))]
        public ActionResult<DtoUser> CreateSpace(DtoSpacePost post)
        {
            var space = new Space(post.Alias, []);
            var dtoSpace = new DtoSpace(space.Alias, space.Guid, [.. space.Members.Select(member => member.Guid)]);
            return CreatedAtAction(null, dtoSpace);
        }

        //AddUserToSpace (Put)
        //DtoUser user, Guid spaceGuid => DtoSpace (side effect)

        //RemoveUserFromSpace (Put)
        //Guid userGuid, Guid spaceGuid => DtoSpace  (side effect)

        //GetSpacesByUserGuid (Get)
        //Guid userGuid => List<DtoSpace>

        //GetMessagesInSpaceBeforeDate (Get)
        //Guid spaceGuid, DateTime? beforeDate, int? numberOfMessages => DtoMessageSequence

        //CreateMessage (Post)
        //Guid userGuid, Guid spaceGuid, DtoMessagePost => DtoMessage


    }
}