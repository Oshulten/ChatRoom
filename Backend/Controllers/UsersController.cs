using Backend.Models;
using Backend.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ChatroomDatabaseContext db) : ControllerBase
{
}