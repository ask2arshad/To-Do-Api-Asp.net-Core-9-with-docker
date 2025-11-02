using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoContext _db;

    public TodoController(TodoContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        => await _db.Todos.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItem>> Get(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        return todo is null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem item)
    {
        _db.Todos.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TodoItem item)
    {
        if (id != item.Id) return BadRequest();

        var exists = await _db.Todos.AnyAsync(t => t.Id == id);
        if (!exists) return NotFound();

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo is null) return NotFound();
        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
