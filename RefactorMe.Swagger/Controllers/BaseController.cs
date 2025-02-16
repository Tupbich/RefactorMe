using Microsoft.AspNetCore.Mvc;
using RefactorMe.MsSql.Repositories;

namespace RefactorMe.Swagger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController<T>(IBaseRepository<T> repository) : ControllerBase
    where T : class
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        return Ok(await repository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<T>> Get(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<T>> Create(T entity)
    {
        var createdEntity = await repository.AddAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = createdEntity.GetType().GetProperty("Id")?.GetValue(createdEntity) }, createdEntity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, T entity)
    {
        if ((int)entity.GetType().GetProperty("Id")?.GetValue(entity)! != id)
            return BadRequest();

        await repository.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}