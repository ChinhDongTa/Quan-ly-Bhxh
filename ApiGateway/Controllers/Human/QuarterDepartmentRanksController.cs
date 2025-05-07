using ApiGateway.Helpers;
using DataServices.Data;
using DataTranfer.Mapping;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.BaseDapper;
using DongTa.QuarterInYear;
using DongTa.ResponseMessage;
using DongTa.ResponseResult;
using Dtos.Human;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
[Authorize]
public class QuarterDepartmentRanksController : ControllerBase {
    private readonly BhxhDbContext context;
    private readonly IGenericDapper dapper;

    public QuarterDepartmentRanksController(BhxhDbContext context, IGenericDapper dapper)
    {
        this.context = context;
        this.dapper = dapper;
        this.dapper.DbNameType = DatabaseNameType.Employee;
    }

    // GET: QuarterDepartmentRanks
    [HttpGet("GetByQuarter/{q}/{year}")]
    public async Task<IActionResult> GetByQuarter(byte q, int year)
    {
        var quarter = new QuarterInYear(q, year);
        var list = await context.QuarterDepartmentRanks
            .Where(x => x.Quarter == q && x.Year == year)
            .Include(x => x.Dept).Include(x => x.Reward)
            .Select(x => x.ToDto()).ToListAsync();
        //var deptIds = await dapper.GetByQueryAsync<int>("select Id, ShortName from Departments where IsActivity = 1");
        var dept = await context.Departments.Where(x => x.IsActive == true).ToListAsync();

        foreach (var item in dept!)
        {
            var existDepId = list.Any(x => x.DeptId == item.Id);
            if (existDepId is false)
            {
                list.Add(new QuarterDepartmentRankDto
                {
                    Id = 0,
                    DeptId = item.Id,
                    DeptName = item.Name,
                    RewardId = 0,
                    RewardName = "",
                    Quarter = q,
                    Year = year,
                    SelfScore = 0,
                    ResultScore = 0,
                    BaseCore = 0,
                    Note = ""
                });
            }
        }
        return Ok(ResultExtension.GetResult(list));
    }

    /// <summary>
    /// Lấy kết quả xếp loại quý hiện tại của 1 đơn vị theo userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("GetCurrentByUserId/{userId}")]
    public async Task<IActionResult> GetCurrentByUserId(string userId)
    {
        return Ok(ResultExtension.GetResult(await context.GetCurrentQuarterDepartmentRankDtoByUserId(dapper, userId)));
    }

    /// <summary>
    /// Lấy kết quả xếp loại quý hiện tại của 1 đơn vị theo deptId
    /// </summary>
    /// <param name="deptId"></param>
    /// <returns></returns>
    [HttpGet("GetCurrentByDeptId/{deptId}")]
    public async Task<IActionResult> GetCurrentByDeptId(int deptId)
    {
        return Ok(ResultExtension.GetResult(await context.GetCurrentQuarterDepartmentRankDtoByDeptId(deptId)));
    }

    // GET: api/QuarterDepartmentRanks/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        return Ok(ResultExtension.GetResult(await context.QuarterDepartmentRanks.FindAsync(id)));
    }

    // PUT: api/QuarterDepartmentRanks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(ActionBase.Update + "/{id}")]
    public async Task<IActionResult> Update(int id, QuarterDepartmentRankDto dto)
    {
        if (id != dto.Id)
        {
            return Ok(Result<bool>.Failure(InfoMessage.InvalidId(nameof(QuarterDepartmentRankDto))));
        }
        try
        {
            var old = await context.QuarterDepartmentRanks.FindAsync(id);
            if (old is not null)
            {
                old = dto.ToEntity(old);
                context.Entry(old).State = EntityState.Modified;
                return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuarterDepartmentRankExists(id))
            {
                return Ok(Result<bool>.Failure(Message.Notfound));
            }
            else
            {
                throw;
            }
        }
        return Ok(Result<bool>.Failure(Message.None));
    }

    // POST: api/QuarterDepartmentRanks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost(ActionBase.Create)]
    public async Task<IActionResult> Create(QuarterDepartmentRankDto dto)
    {
        if (dto.RewardId == 0)
            dto.RewardId = 20;
        context.QuarterDepartmentRanks.Add(dto.ToEntity());
        try
        {
            return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
        }
        catch (DbUpdateException)
        {
            if (QuarterDepartmentRankExists(dto.Id))
            {
                return Ok(Result<bool>.Failure("Conflict !"));
            }
            else
            {
                throw;
            }
        }
        //return Ok(Result<bool>.Failure(Message.None));
    }

    // DELETE: api/QuarterDepartmentRanks/5
    [HttpDelete(ActionBase.Delete + "{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var quarterDepartmentRank = await context.QuarterDepartmentRanks.FindAsync(id);
        if (quarterDepartmentRank != null)
        {
            context.QuarterDepartmentRanks.Remove(quarterDepartmentRank);
            return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
        }
        return Ok(Result<bool>.Failure(Message.Notfound));
    }

    private bool QuarterDepartmentRankExists(int id)
    {
        return context.QuarterDepartmentRanks.Any(e => e.Id == id);
    }
}