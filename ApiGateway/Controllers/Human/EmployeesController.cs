using DataServices.Data;
using Dtos.Human;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.BaseDapper;
using DongTa.ResponseResult;
using DataTranfer.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DataServices.Entities.Human;
using Microsoft.AspNetCore.Identity;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase {
    private readonly BhxhDbContext context;
    private readonly IGenericDapper dapper;
    private readonly UserManager<ApiUser> userManager;

    public EmployeesController(BhxhDbContext context, IGenericDapper dapper, UserManager<ApiUser> userManager)
    {
        this.context = context;
        this.dapper = dapper;
        this.dapper.DbNameType = DatabaseNameType.Employee;
        this.userManager = userManager;
    }

    // GET: api/Employees
    [HttpGet(ActionBase.All)]
    public async Task<IActionResult> All()
    {
        return Ok(ResultExtension.GetResult(
            await context.Employees.Where(x => x.IsQuitJob == false)
            .Include(x => x.SalaryCoefficient).Include(x => x.Dept).Include(x => x.Post)
                                    .OrderBy(x => x.SortOrder).ThenBy(x => x.Birthdate)
                                   .Select(x => x.ToDto()).ToListAsync())
            );
    }

    [HttpGet("GetBirthdayInMonth/{month}/{isAll}")]
    public async Task<IActionResult> GetBirthdayInMonth(int month, bool isAll = false)
    {
        if (isAll)
        {
            return Ok(ResultExtension.GetResult(
                await context.Employees
                    .Where(x => x.Birthdate.HasValue && x.Birthdate.Value.Month == month && x.IsQuitJob == false)
                    .Include(x => x.SalaryCoefficient).Include(x => x.Dept).Include(x => x.Post)
                    .OrderBy(x => x.SortOrder).ThenBy(x => x.Birthdate)
                    .Select(x => x.ToDto()).ToListAsync())
                );
        }
        else
        {
            return Ok(ResultExtension.GetResult(
                await context.Employees
                    .Where(x => x.Birthdate.HasValue && x.Birthdate.Value.Month == month
                        && x.IsQuitJob == false && x.Dept != null && x.Dept.LevelId == 2) // Added null check for x.Dept
                    .Include(x => x.SalaryCoefficient).Include(x => x.Dept).Include(x => x.Post)
                    .OrderBy(x => x.SortOrder).ThenBy(x => x.Birthdate)
                    .Select(x => x.ToDto()).ToListAsync())
                );
        }
    }

    [HttpGet("FindByName/{name}/{quitJob}")]
    public async Task<IActionResult> FindByName(string name, bool quitJob = false)
    {
        return quitJob
            ? Ok(ResultExtension.GetResult(await context.Employees
                    .Where(x => x.FirstName.Contains(name) || x.LastName.Contains(name))
                    .Include(x => x.SalaryCoefficient)
                    .Include(x => x.Dept)
                    .Include(x => x.Post)
                    .OrderBy(x => x.SortOrder).ThenBy(x => x.Birthdate)
                    .Select(x => x.ToDto()).ToListAsync())
            )
            : Ok(ResultExtension.GetResult(await context.Employees
                    .Where(x => x.IsQuitJob == false && (x.FirstName.Contains(name) || x.LastName.Contains(name)))
                    .Include(x => x.SalaryCoefficient)
                    .Include(x => x.Dept)
                    .Include(x => x.Post)
                    .OrderBy(x => x.SortOrder).ThenBy(x => x.Birthdate)
                    .Select(x => x.ToDto()).ToListAsync())
            );
    }

    // GET: api/Employees/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var employee = await context.Employees.Include(x => x.SalaryCoefficient)
            .Include(x => x.Dept).Include(x => x.Post)
                                .FirstOrDefaultAsync(x => x.Id == id);

        return Ok(ResultExtension.GetResult(employee?.ToDto()));
    }

    [HttpGet("GetEmployeeDto")]
    public async Task<IActionResult> GetEmployeeDto()
    {
        var dto = await dapper.GetOneAsync<EmployeeDto>(SqlQueryString.SelectEmployeeDto);
        //FormattableString query = ;

        return Ok(ResultExtension.GetResult(dto));
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetoByUserId(string userId) => Ok(ResultExtension.GetResult(
            await dapper.GetOneAsync<EmployeeDto>(SqlQueryString.SelectEmployeeDtoByUserId(userId)))
            );

    /// <summary>
    /// Lấy người đứng đầu phòng ban theo userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("GetDeptHeadByUserId/{userId}")]
    public async Task<IActionResult> GetDeptHeadByUserId(string userId)
    {
        return Ok(ResultExtension.GetResult(
            await dapper.GetOneAsync<EmployeeDto>(SqlQueryString.GetDeptHeadByUserId(userId))));
    }

    [HttpGet("GetByDeptId/{deptId}")]
    public async Task<IActionResult> GetByDeptId(int deptId)
    {
        return Ok(ResultExtension.GetResult(
            await context.Employees.Where(x => x.DeptId == deptId && x.IsQuitJob == false)
                                    .Include(x => x.SalaryCoefficient).Include(x => x.Dept).Include(x => x.Post)
                                    .OrderBy(x => x.SortOrder).ThenBy(x => x.Birthdate)
                                    .Select(x => x.ToDto()).ToListAsync())
            );
    }

    [HttpGet("GetEmployeeDtoForListBox/{name}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEmployeeDtoForListBox(string name = "All")
    {
        if (name == "All")
        {
            var result = await context.Employees.Where(s => s.IsQuitJob == false)
                .Include(s => s.Dept)
                .AsSplitQuery()
                .OrderBy(s => s.FirstName).ThenBy(s => s.LastName)
                .Select(s => new EmployeeDtoForListBox(s.Id, $"{s.FirstName} {s.LastName}-{s.Dept!.ShortName}"))
                .ToListAsync();

            return Ok(ResultExtension.GetResult(result));
        }
        else
        {
            var result = await context.Employees.Where(s => s.IsQuitJob == false && (s.FirstName.Contains(name) || s.LastName.Contains(name)))
                .Include(s => s.Dept)
                .AsSplitQuery()
                .OrderBy(s => s.FirstName).ThenBy(s => s.LastName)
                .Select(s => new EmployeeDtoForListBox(s.Id, $"{s.FirstName} {s.LastName}-{s.Dept!.ShortName}"))
                .ToListAsync();
            //FormattableString query = ;
            return Ok(ResultExtension.GetResult(result));
        }
    }

    // PUT: api/Employees/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(ActionBase.Update + "/{id}")]
    public async Task<IActionResult> Update(int id, EmployeeDto dto)
    {
        if (id != dto.Id)
        {
            return Ok(Result<bool>.Failure(DongTa.ResponseMessage.Message.Notfound));
        }

        try
        {
            var employee = context.Employees.Find(id);
            if (employee is not null)
            {
                employee = dto.ToEntity(employee);
                context.Entry(employee).State = EntityState.Modified;
                var success = await context.SaveChangesAsync() > 0;
                if (success)
                {
                    await CreateEventLogAsync("Update", "Cập nhật nhân viên");
                    return Ok(Result<bool>.Success(InfoMessage.Success));
                }
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
            {
                return Ok(Result<bool>.Failure(DongTa.ResponseMessage.Message.Notfound));
            }
            else
            {
                throw;
            }
        }
        return Ok(Result<bool>.Failure(DongTa.ResponseMessage.Message.Notfound));
    }

    // POST: api/Employees
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost(ActionBase.Create)]
    public async Task<IActionResult> Create(EmployeeDto dto)
    {
        context.Employees.Add(dto.ToEntity());
        var success = await context.SaveChangesAsync() > 0;
        if (success)
        {
            await CreateEventLogAsync("Create", "Thêm mới nhân viên");
            return Ok(Result<bool>.BoolResult(success));
        }
        return Ok(Result<bool>.Failure("Tạo mới nhân viên thất bại"));
    }

    // DELETE: api/Employees/5
    [HttpDelete(ActionBase.Delete + "/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null)
        {
            return Ok(Result<bool>.Failure(DongTa.ResponseMessage.Message.Notfound));
        }
        context.Employees.Remove(employee);
        return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
    }

    private async Task CreateEventLogAsync(string actionName, string description)
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            var eventLog = new EventLog
            {
                UserId = user.Id,
                CreateTime = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Browser = HttpContext.Request.Headers.UserAgent, // Simplified Substring
                ActionName = actionName,
                Description = description
            };
            context.EventLogs.Add(eventLog);
            await context.SaveChangesAsync();
        }
    }

    private bool EmployeeExists(int id)
    {
        return context.Employees.Any(e => e.Id == id);
    }
}