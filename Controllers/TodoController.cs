using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Controllers;

[ApiController]
[Route("v1/todos")]
public class TodoController: ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get(
        [FromServices] AppDbContext context
        )
    {
        var todos = await context.Todos.AsNoTracking().ToListAsync();
        
        return Ok(todos);
    }   
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(
        [FromServices] AppDbContext context,
        [FromRoute] int id
        )
    {
        var todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(todo => todo.Id == id);
        
        return todo == null ? NotFound() : Ok(todo);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> PostAsync(
        [FromServices] AppDbContext context,
        [FromBody] CreateTodoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var todo = new Todo
        {
            Date = DateTime.Now,
            Done = false,
            Title = model.Title
        };

        try
        {
            await context.Todos.AddAsync(todo);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return Created("v1/todos", todo);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id,
        [FromBody] UpdateTodoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(todo => todo.Id == id);
        
        if (todo == null)
        {
            return NotFound();
        }

        todo.Title = model.Title;
        
        try
        {
            context.Todos.Update(todo);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok(todo);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id
        )
    {
   
        var todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(todo => todo.Id == id);

        if (todo == null)
        {
            return NotFound();
        }
        
        try
        {
            context.Todos.Remove(todo);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok();
    }
}