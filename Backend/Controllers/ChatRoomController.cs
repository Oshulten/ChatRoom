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
        //CreateUserByAuth (Post)
        //DtoAuthorization auth => DtoUser (side effect)
        //Tested (3)
        [HttpPost("create-user")]
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

        //GetUserByAuth (Get)
        //DtoAuthorization auth => DtoUser

        //CreateSpace (Post)
        //DtoSpacePost => DtoSpace (side effect)

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