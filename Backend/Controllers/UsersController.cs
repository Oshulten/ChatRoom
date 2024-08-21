using Backend.Models;
using Backend.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DtoUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<DtoUser> GetUserByIds(Guid userId)
    {
        var existingUser = db.Users.FirstOrDefault(user => user.Id == userId);
        if (existingUser is null)
        {
            return NotFound();
        }
        return (DtoUser)existingUser;
    }
}