using DataServices.Data;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.ResponseResult;
using DataTranfer.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataServices.Entities.Human;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
[Authorize]
public class PositionsController(BhxhDbContext context) : ControllerBase {

    // GET: api/Positions
    [HttpGet(ActionBase.All)]
    public async Task<IActionResult> All()
    {
        return Ok(ResultExtension.GetResult(await context.Positions.Select(x => x.ToDto()).ToListAsync()));
    }

    // GET: api/Positions/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<ActionResult<Position>> GetOne(int id)
    {
        var position = await context.Positions.FindAsync(id);

        return Ok(ResultExtension.GetResult(position?.ToDto()));
    }

    // PUT: api/Positions/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(ActionBase.Update + "/{id}")]
    public async Task<IActionResult> Update(int id, Position position)
    {
        if (id != position.PositionId)
        {
            return BadRequest();
        }

        context.Entry(position).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PositionExists(id))
            {
                return Ok(Result<bool>.Failure(InfoMessage.InvalidId(nameof(Position))));
            }
            else
            {
                throw;
            }
        }

        return Ok(Result<bool>.Failure("No Content"));
    }

    // POST: api/Positions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost(ActionBase.Create)]
    public async Task<ActionResult<Position>> Create(Position position)
    {
        context.Positions.Add(position);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetPosition", new { id = position.PositionId }, position);
    }

    // DELETE: api/Positions/5
    [HttpDelete(ActionBase.Delete + "/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var position = await context.Positions.FindAsync(id);
        if (position == null)
        {
            return NotFound();
        }

        context.Positions.Remove(position);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool PositionExists(int id)
    {
        return context.Positions.Any(e => e.PositionId == id);
    }
}