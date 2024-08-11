using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SeedDatabaseController(ChatroomDatabaseContext db) : ControllerBase
{
    [HttpPost]
    public IActionResult SeedData()
    {
        db.ChatMessages.RemoveRange(db.ChatMessages);
        db.ChatSpaces.RemoveRange(db.ChatSpaces);
        db.ChatUsers.RemoveRange(db.ChatUsers);
        return Ok();
    }
}