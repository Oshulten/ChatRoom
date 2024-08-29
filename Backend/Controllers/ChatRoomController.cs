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
        private static DtoUser ToDtoUser(User user) =>
            new(user.Guid, user.Alias, user.JoinedAt, user.Admin);

        private static DtoSpace ToDtoSpace(Space space) =>
            new(space.Alias, space.Guid, space.Members.Select(m => m.Guid).ToList());

        private static DtoMessage ToDtoMessage(Message message) =>
            new(message.Content, message.Space.Guid, message.Sender.Guid, message.PostedAt);

        private static User ToUser(DtoAuthentication auth) =>
            new(auth.Alias, auth.Password);

        private static Space ToSpace(DtoSpacePost space) =>
            new(space.Alias);

        private Message ToMessage(DtoMessage message)
        {
            var user = context.UserByGuid(message.SenderGuid);
            var space = context.SpaceByGuid(message.SpaceGuid);

            if (user is null || space is null)
                throw new Exception("User or space doesn't exist");

            return new Message(user, message.PostedAt, message.Content, space);
        }

        [HttpPost("seed")]
        [ProducesResponseType(200)]
        public IActionResult Seed()
        {
            context.SeedData(2, 2, 15);
            return Ok();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(200)]
        public IActionResult Clear()
        {
            context.Clear();

            return Ok();
        }

        [HttpGet("get-users")]
        public List<DtoUser> GetDtoUsers() =>
            [.. context.Users.Select(user => ToDtoUser(user))];

        [HttpGet("get-spaces")]
        public List<DtoSpace> GetDtoSpaces()
        {
            foreach (var space in context.Spaces)
            {
                Console.WriteLine(space.Members.Count);
                var dto = ToDtoSpace(space);
                Console.WriteLine(dto.MemberGuids);
            }
            return [.. context.Spaces.Select(space => ToDtoSpace(space))];
        }


        [HttpGet("get-messages")]
        public List<DtoMessage> GetDtoMessages() =>
            [.. context.Messages.Select(message => ToDtoMessage(message))];

        //Tested (3)
        [HttpPost("create-user")]
        [ProducesResponseType(201, Type = typeof(DtoUser))]
        [ProducesResponseType(400)]
        public ActionResult<DtoUser> CreateUserByAuth(DtoAuthentication auth)
        {
            var user = context.CreateUser(ToUser(auth));
            return user != null
                ? CreatedAtAction(null, ToDtoUser(user))
                : BadRequest($"A user with alias {auth.Alias} already exists.");
        }

        //Tested (3)
        [HttpPost("get-user-by-auth")]
        [ProducesResponseType(200, Type = typeof(DtoUser))]
        [ProducesResponseType(400)]
        public ActionResult<DtoUser> GetUserByAuth(DtoAuthentication auth)
        {
            var user = context.UserByAuth(auth);

            return user == null
                ? BadRequest($"A user with that alias and password does not exist")
                : Ok(ToDtoUser(user));
        }

        //Tested (2)
        [HttpPost("create-space")]
        [ProducesResponseType(201, Type = typeof(DtoSpace))]
        [ProducesResponseType(400)]
        public ActionResult<DtoSpace> CreateSpace(DtoSpacePost dto)
        {
            var space = ToSpace(dto);

            return context.CreateSpace(space)
                ? CreatedAtAction(null, ToDtoSpace(space))
                : BadRequest("A space with that guid already exists");
        }

        [HttpPut("add-user-to-space/{spaceGuid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public ActionResult<DtoSpace> AddUserToSpace([FromRoute] Guid spaceGuid, [FromQuery] Guid userGuid)
        {
            return context.AddUserToSpace(userGuid, spaceGuid)
                ? Ok(ToDtoSpace(context.SpaceByGuid(spaceGuid)!))
                : BadRequest();
        }

        [HttpGet("get-spaces-by-user-guid/{userGuid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(List<DtoSpace>))]
        public ActionResult<List<DtoSpace>> GetSpacesByUserGuid([FromRoute] Guid userGuid)
        {

            var spaces = context.SpacesByUserGuid(userGuid);

            return spaces != null
                ? Ok(spaces.Select(ToDtoSpace))
                : NotFound($"A user with guid {userGuid} does not exist");
        }

        [HttpPost("create-message")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201, Type = typeof(DtoMessage))]
        public ActionResult<DtoMessage> CreateMessage(DtoMessage post)
        {
            var message = context.CreateMessage(ToMessage(post));

            return message != null
                ? CreatedAtAction(null, ToDtoMessage(message))
                : BadRequest("User or space does not exist");
        }

        //GetMessagesInSpaceBeforeDate (Get)
        //Guid spaceGuid, DateTime? beforeDate, int? numberOfMessages => DtoMessageSequence
        [HttpGet("get-messages-in-space/{spaceGuid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<DtoMessage>))]
        public ActionResult<List<DtoMessage>> GetMessagesInSpace(Guid spaceGuid)
        {
            var messages = context.MessagesBySpaceGuid(spaceGuid, null, null);

            return messages != null
                ? Ok(messages.Select(ToDtoMessage))
                : BadRequest("Space doesn't exist");
        }

        [HttpGet("get-messages-in-space-before-date/{spaceGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessageSequence))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoMessageSequence> GetBySpaceAndDate(Guid spaceGuid, [FromQuery] DateTime? messagesBefore, [FromQuery] int? numberOfMessages)
        {
            var messages = context.MessagesBySpaceGuid(spaceGuid, messagesBefore, numberOfMessages);

            var dtoMessages = messages!
                .Select(ToDtoMessage)
                .ToList();

            var distinctUsers = dtoMessages
                .Select(message => message.SenderGuid)
                .Distinct()
                .Select(guid => context.Users.FirstOrDefault(user => user.Guid == guid)!);

            if (distinctUsers.Any(user => user is null))
            {
                return Problem("Corrupt database");
            }

            var dtoUsers = distinctUsers.Select(ToDtoUser);

            var earliestMessage = dtoMessages[^1];
            var lastMessage = dtoMessages[0];
            var earliest = earliestMessage == dtoMessages[0];

            if (numberOfMessages is null)
            {
                earliest = true;
            }
            else
            {
                earliest = dtoMessages.Count < numberOfMessages;
            }

            return new DtoMessageSequence(
                earliestMessage.PostedAt,
                lastMessage.PostedAt,
                earliest,
                dtoMessages,
                dtoUsers.ToList()
            );
        }
    }
}