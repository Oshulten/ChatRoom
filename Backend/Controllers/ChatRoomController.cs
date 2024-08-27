using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Dto;
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
        [HttpPost]
        public async Task<ActionResult<DtoUser>> CreateUserByAuth(DtoAuthentication auth)
        {
            var createdUser = await context.CreateUser(auth);

            if (createdUser is null)
                return BadRequest($"A user with alias {auth.Alias} already exists.");

            return CreatedAtAction(null, context.DtoUserFromUser(createdUser));
        }

        //GetUserByAuth (Get)
        //DtoAuthorization auth => DtoUser
        [HttpPost]
        public async Task<ActionResult<DtoUser>> GetUserByAuth(DtoAuthentication auth)
        {
            var existingUser = await context.GetUserFromAuthentication(auth);

            if (existingUser is null)
                return BadRequest($"A user with alias {auth.Alias} and password {auth.Password} cannot be found.");

            return Ok(context.DtoUserFromUser(existingUser));
        }

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