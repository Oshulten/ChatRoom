using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

/* 
To copy-paste this class for use with another database table
- Rename the class name manually
- Search and replace "DefaultTypeDto" with '{table-data-type}Dto'
- Search and replace "DefaultData" with '{new-table-name}'
*/

[ApiController]
[Route("api/[controller]")]
public class TemplateController : ControllerBase
{
    private readonly string _deleteAllKey = "clear-all";

    [HttpGet("{id}")]
    public ActionResult<DefaultDataTypeView> GetById(Guid id, CustomDatabaseContext db)
    {
        DefaultDataType? entity = db.DefaultDataTable.FirstOrDefault(data => data.Id == id);
        return entity is not null ? Ok(entity) : NotFound();
    }

    [HttpGet]
    public ActionResult<List<DefaultDataTypeView>> GetAll(CustomDatabaseContext db)
    {
        if (!db.DefaultDataTable.Any())
        {
            db.DefaultDataTable!.AddRange(DefaultDataType.SeedData(10));
        }
        db.SaveChanges();
        var entities = db.DefaultDataTable.ToList().Select(entity => entity);
        return Ok(entities);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchById(Guid id, DefaultDataType patchObject, CustomDatabaseContext db)
    {
        DefaultDataType? entity = db.DefaultDataTable.FirstOrDefault(data => data.Id == id);
        if (entity is null)
        {
            return NotFound();
        }
        entity.Patch(patchObject);
        db.SaveChanges();
        return Ok();
    }

    [HttpPost]
    public IActionResult Post(DefaultDataTypeView dto, CustomDatabaseContext db)
    {
        var entity = (DefaultDataType)dto;
        db.Add<DefaultDataType>(entity);
        db.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, null);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteById(Guid id, CustomDatabaseContext db)
    {
        DefaultDataType? entity = db.DefaultDataTable.FirstOrDefault(data => data.Id == id);
        if (entity is null)
        {
            return NotFound();
        }
        db.Remove<DefaultDataType>(entity);
        db.SaveChanges();
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteAll(string deleteAllKey, CustomDatabaseContext db)
    {
        if (deleteAllKey != _deleteAllKey)
        {
            return Unauthorized();
        }

        db.RemoveRange(db.DefaultDataTable.ToList());
        db.SaveChanges();
        return Ok();
    }
}