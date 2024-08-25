using Backend.Database;
using Backend.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet("{userGuid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoUser> GetUserByGuid(Guid userGuid)
    {
        var existingUser = db.Users.FirstOrDefault(user => user.Guid == userGuid);
        if (existingUser is null)
        {
            return NotFound();
        }
        return (DtoUser)existingUser;
    }
}