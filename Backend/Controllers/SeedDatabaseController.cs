using Backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SeedDatabaseController(ChatroomDatabaseContext context) : ControllerBase
{
    [HttpPost]
    public IActionResult SeedData(int numberOfUsers, int numberOfSpaces, int numberOfMessages)
    {
        context.SeedData(numberOfUsers, numberOfSpaces, numberOfMessages);
        return Ok();
    }
}