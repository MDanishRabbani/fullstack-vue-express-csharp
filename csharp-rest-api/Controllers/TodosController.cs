using Microsoft.AspNetCore.Mvc;
using csharp_rest_api.DTOs;
using csharp_rest_api.Models;
using csharp_rest_api.Services;

namespace csharp_rest_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<TodoItem>> GetAll()
    {
        return Ok(_todoService.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TodoItem> GetById(int id)
    {
        var item = _todoService.GetById(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TodoItem> Create([FromBody] CreateTodoRequest request)
    {
        var item = _todoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TodoItem> Update(int id, [FromBody] CreateTodoRequest request)
    {
        var item = _todoService.Update(id, request);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var removed = _todoService.Delete(id);
        if (!removed) return NotFound();
        return NoContent();
    }
}
