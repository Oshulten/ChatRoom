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

        private Message ToMessage(DtoMessage post)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Guid == post.SenderGuid);
            var existingSpace = context.Spaces.FirstOrDefault(s => s.Guid == post.SpaceGuid);

            if (existingUser is null || existingSpace is null)
                throw new Exception("User or space doesn't exist");

            return new Message(existingUser, post.PostedAt, post.Content, existingSpace);
        }

        [HttpPost("clear")]
        [ProducesResponseType(200)]
        public IActionResult Clear()
        {
            context.Users.RemoveRange(context.Users);
            context.Spaces.RemoveRange(context.Spaces);
            context.Messages.RemoveRange(context.Messages);

            context.SaveChanges();

            return Ok();
        }

        [HttpGet("get-users")]
        public List<DtoUser> GetUsers() =>
            [.. context.Users.Select(user => ToDtoUser(user))];

        [HttpGet("get-spaces")]
        public List<DtoSpace> GetSpaces() =>
            [.. context.Spaces.Select(space => ToDtoSpace(space))];

        [HttpGet("get-messages")]
        public List<DtoMessage> GetMessages() =>
            [.. context.Messages.Select(message => ToDtoMessage(message))];

        //Tested (3)
        [HttpPost("create-user")]
        [ProducesResponseType(201, Type = typeof(DtoUser))]
        [ProducesResponseType(400)]
        public ActionResult<DtoUser> CreateUserByAuth(DtoAuthentication auth)
        {
            var existingUser = context.Users.FirstOrDefault(user => user.Alias == auth.Alias);

            if (existingUser is null)
            {
                var user = ToUser(auth);

                context.Users.Add(user);
                context.SaveChanges();

                return CreatedAtAction(null, ToDtoUser(user));
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

            return Ok(ToDtoUser(matchingUser));
        }

        //Tested (2)
        [HttpPost("create-space")]
        [ProducesResponseType(201, Type = typeof(DtoSpace))]
        public ActionResult<DtoUser> CreateSpace(DtoSpacePost post)
        {
            var space = ToSpace(post);

            context.Spaces.Add(space);
            context.SaveChanges();

            return CreatedAtAction(null, ToDtoSpace(space));
        }

        [HttpPut("add-user-to-space/{spaceGuid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(DtoSpace))]
        public ActionResult<DtoSpace> AddUserToSpace([FromRoute] Guid spaceGuid, [FromQuery] Guid userGuid)
        {
            var existingSpace = context.Spaces.FirstOrDefault(space => space.Guid == spaceGuid);

            if (existingSpace is null)
                return NotFound($"A space with guid {spaceGuid} does not exist");

            var existingUser = context.Users.FirstOrDefault(user => user.Guid == userGuid);

            if (existingUser is null)
                return NotFound($"A user with guid {userGuid} does not exist");

            var existingMemberInSpace = existingSpace.Members.FirstOrDefault(member => member.Guid == existingUser.Guid);

            if (existingMemberInSpace is null)
            {
                existingSpace.Members.Add(existingUser);
                existingUser.Spaces.Add(existingSpace);
                context.SaveChanges();
            }

            return Ok(ToDtoSpace(existingSpace));
        }

        [HttpGet("get-spaces-by-user-guid/{userGuid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(List<DtoSpace>))]
        public ActionResult<List<DtoSpace>> GetSpacesByUserGuid([FromRoute] Guid userGuid)
        {
            var existingUser = context.Users.FirstOrDefault(user => user.Guid == userGuid);

            if (existingUser is null)
                return NotFound($"A user with guid {userGuid} does not exist");

            var memberSpaces = context.Spaces
                .Where(space => space.Members.Contains(existingUser))
                .Select(space => ToDtoSpace(space))
                .ToList();

            return Ok(memberSpaces);
        }

        [HttpPost("create-message")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201, Type = typeof(DtoMessage))]
        public ActionResult<DtoMessage> CreateMessage(DtoMessage post)
        {
            var existingSpace = context.Spaces.FirstOrDefault(s => s.Guid == post.SpaceGuid);
            var existingUser = context.Users.FirstOrDefault(u => u.Guid == post.SenderGuid);

            if (existingSpace is null || existingUser is null)
                return BadRequest("Space or user doesn't exist");

            var message = ToMessage(post);

            existingSpace.Messages.Add(message);
            existingUser.Messages.Add(message);

            context.Messages.Add(message);
            context.SaveChanges();

            return CreatedAtAction(null, ToDtoMessage(message));
        }

        //GetMessagesInSpaceBeforeDate (Get)
        //Guid spaceGuid, DateTime? beforeDate, int? numberOfMessages => DtoMessageSequence
        [HttpGet("get-messages-in-space/{spaceGuid:guid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(List<DtoMessage>))]
        public ActionResult<List<DtoMessage>> GetMessagesInSpace(Guid spaceGuid)
        {
            var existingSpace = context.Spaces.FirstOrDefault(s => s.Guid == spaceGuid);

            if (existingSpace is null)
                return BadRequest("Space doesn't exist");

            return Ok(existingSpace.Messages.Select(m => ToDtoMessage(m)).ToList());
        }

        // [HttpGet("{spaceGuid}")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoMessageSequence))]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public ActionResult<DtoMessageSequence> GetBySpaceAndDate(Guid spaceGuid, [FromQuery] DateTime? messagesBefore, [FromQuery] int? numberOfMessages)
        // {
        //     if (context.SpaceByGuid(spaceGuid) is null)
        //     {
        //         return NotFound($"A space with guid {spaceGuid} doesn't exist");
        //     }

        //     var messages = context.Messages
        //         .Where(message => message.Space.Guid == spaceGuid)
        //         .OrderByDescending(message => message.PostedAt)
        //         .ToList();

        //     if (messagesBefore is not null)
        //     {
        //         messages = messages.Where(message => message.PostedAt < messagesBefore).ToList();
        //     }

        //     if (numberOfMessages is not null)
        //     {
        //         messages = messages.Take(numberOfMessages ?? 0).ToList();
        //     }

        //     var dtoMessages = messages
        //         .Select(message => (DtoMessage)message)
        //         .ToList();

        //     var distinctUsers = dtoMessages
        //         .Select(message => message.SenderGuid)
        //         .Distinct()
        //         .Select(guid => context.Users.FirstOrDefault(user => user.Guid == guid));

        //     if (distinctUsers.Any(user => user is null))
        //     {
        //         return Problem("Corrupt database");
        //     }

        //     var dtoUsers = distinctUsers.Select(user => (DtoUser)user!);

        //     var earliestMessage = dtoMessages[^1];
        //     var lastMessage = dtoMessages[0];
        //     var earliest = earliestMessage == dtoMessages[0];

        //     if (numberOfMessages is null)
        //     {
        //         earliest = true;
        //     }
        //     else
        //     {
        //         earliest = dtoMessages.Count < numberOfMessages;
        //     }

        //     return new DtoMessageSequence(
        //         earliestMessage.PostedAt,
        //         lastMessage.PostedAt,
        //         earliest,
        //         dtoMessages,
        //         dtoUsers.ToList()
        //     );
        // }
    }
}