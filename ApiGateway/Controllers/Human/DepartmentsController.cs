using DataServices.Data;
using DataServices.Entities.Human;
using DataTranfer.Dtos;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.BaseDapper;
using DongTa.ResponseResult;
using DataTranfer.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
public class DepartmentsController(BhxhDbContext context, IGenericDapper dapper) : ControllerBase {

    // GET: Departments/All
    [HttpGet(ActionBase.All)]
    public async Task<IActionResult> All()
    {
        return Ok(ResultExtension.GetResult(
            await context.Departments.OrderBy(x => x.SortOrder)
                    .Include(x => x.Level)
                    .Select(x => x.ToDto()).ToListAsync()));
    }

    // GET: Departments/GetOne/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var department = await context.Departments.FindAsync(id);
        return Ok(ResultExtension.GetResult(department?.ToDto()));
    }

    // GET: Departments/GetOne/5
    [HttpGet("GetOneByEmployeeId/{employeeId}")]
    public async Task<IActionResult> GetOneByEmployeeId(int employeeId)
    {
        return Ok(ResultExtension.GetResult(await dapper.GetOneAsync<DepartmentDto>(SqlQueryString.SelectDepartmentDtoByEmployeeId(employeeId))));
    }

    // PUT: api/Departments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(ActionBase.Update + "/{id}")]
    public async Task<IActionResult> Update(int id, DepartmentDto department)
    {
        if (id != department.Id)
        {
            return Ok(Result<bool>.Failure(InfoMessage.InvalidId(nameof(Department))));
        }

        try
        {
            var dept = context.Departments.Find(id);
            if (dept is not null)
            {
                context.Entry(dept).State = EntityState.Modified;
                dept = department.ToEntity(dept);
                var ok = await context.SaveChangesAsync() > 0;
                return Ok(ResultExtension.GetResult(ok, InfoMessage.CrudResult(CRUD.Update, ok, nameof(DepartmentDto))));
            }
            return Ok(Result<bool>.Failure(InfoMessage.NotFound));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartmentExists(id))
            {
                return Ok(Result<bool>.Failure(InfoMessage.ObjectNull(nameof(Department))));
            }
            else
            {
                throw;
            }
        }
    }

    // POST: api/Departments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost(ActionBase.Create)]
    public async Task<ActionResult<Department>> Create(DepartmentDto department)
    {
        context.Departments.Add(department.ToEntity());
        return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
    }

    // DELETE: Departments/5
    [HttpDelete(ActionBase.Delete + "/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await context.Departments.FindAsync(id);
        if (department == null)
        {
            return Ok(Result<bool>.Failure(InfoMessage.NotFound));
        }
        context.Departments.Remove(department);
        return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
    }

    private bool DepartmentExists(int id)
    {
        return context.Departments.Any(e => e.Id == id);
    }
}