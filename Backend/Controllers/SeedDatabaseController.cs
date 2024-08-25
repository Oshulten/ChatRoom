using Backend.Models;
using Backend.Models.Message;
using Backend.Models.Space;
using Backend.Models.User;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SeedDatabaseController(ChatroomDatabaseContext context) : ControllerBase
{
    [HttpPost]
    public IActionResult SeedData(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        context.SeedDataWithBogus(numberOfUsers, numberOfSpaces, numberOfMessages);
        return Ok();
    }
}