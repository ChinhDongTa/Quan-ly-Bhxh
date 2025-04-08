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
public class SalaryCoefficientsController(BhxhDbContext context) : ControllerBase {

    // GET: api/SalaryCoefficients
    [HttpGet(ActionBase.All)]
    public async Task<IActionResult> All()
    {
        return Ok(ResultExtension.GetResult(await context.SalaryCoefficients.Select(x => x.ToDto()).ToListAsync()));
    }

    // GET: SalaryCoefficients/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<ActionResult<SalaryCoefficient>> GetOne(int id)
    {
        return Ok(ResultExtension.GetResult(await context.SalaryCoefficients.FindAsync(id)));
    }

    // PUT: SalaryCoefficients/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(ActionBase.Update + "/{id}")]
    public async Task<IActionResult> Update(int id, SalaryCoefficient salaryCoefficient)
    {
        if (id != salaryCoefficient.SalaryCoefficientId)
        {
            return Ok(Result<bool>.Failure(InfoMessage.InvalidId(nameof(SalaryCoefficient))));
        }

        context.Entry(salaryCoefficient).State = EntityState.Modified;

        try
        {
            var ok = await context.SaveChangesAsync() > 0;
            return Ok(ResultExtension.GetResult(ok, InfoMessage.CrudResult(CRUD.Update, ok, nameof(SalaryCoefficient))));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SalaryCoefficientExists(id))
            {
                return Ok(Result<bool>.Failure(InfoMessage.NotFound));
            }
            else
            {
                throw;
            }
        }
        //return Ok(Result<bool>.Failure("No content"));
    }

    // POST: api/SalaryCoefficients
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost(ActionBase.Create)]
    public async Task<ActionResult<SalaryCoefficient>> Create(SalaryCoefficient salaryCoefficient)
    {
        context.SalaryCoefficients.Add(salaryCoefficient);
        return Ok(ResultExtension.GetResult(await context.SaveChangesAsync() > 0));
    }

    // DELETE: api/SalaryCoefficients/5
    [HttpDelete(ActionBase.Delete + "/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var salaryCoefficient = await context.SalaryCoefficients.FindAsync(id);
        if (salaryCoefficient == null)
        {
            return Ok(Result<bool>.Failure(InfoMessage.NotFound));
        }

        context.SalaryCoefficients.Remove(salaryCoefficient);
        await context.SaveChangesAsync();

        return Ok(Result<bool>.Failure("No content"));
    }

    private bool SalaryCoefficientExists(int id)
    {
        return context.SalaryCoefficients.Any(e => e.SalaryCoefficientId == id);
    }
}