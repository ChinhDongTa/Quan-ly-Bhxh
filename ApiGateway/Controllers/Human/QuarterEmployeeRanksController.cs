using DataServices.Data;
using DataTranfer.Dtos;
using DefaultValue;
using DefaultValue.ApiRoute;
using DongTa.BaseDapper;
using DongTa.ResponseResult;
using DataTranfer.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataServices.Entities.Human;
using DongTa.QuarterInYear;
using DongTa.ResponseMessage;
using ApiGateway.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
[Authorize]
public class QuarterEmployeeRanksController(BhxhDbContext context, IGenericDapper dapper) : ControllerBase {

    #region cá nhân

    // GET: api/QuarterEmployeeRanks
    [HttpGet(ActionBase.All)]
    public async Task<ActionResult<IEnumerable<QuarterEmployeeRank>>> All()
    {
        return await context.QuarterEmployeeRanks.ToListAsync();
    }

    [HttpGet("GetByQuarter/{quarter}/{year}")]
    public async Task<IActionResult> GetByQuarter(byte quarter, int year)
    {
        return Ok(ResultExtension.GetResult(await context.QuarterEmployeeRanks
                    .Where(x => x.Quarter == quarter && x.Year == year)
                    .Select(x => x.ToDto()).ToListAsync()));
    }

    /// <summary>
    /// Lấy kết quả xếp loại quý hiện tại của 1 đơn vị theo userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("GetCurrentByUserId/{userId}")]
    public async Task<IActionResult> GetCurrentByUserId(string userId)
    {
        return Ok(ResultExtension.GetResult(
            await context.GetQuarterEmployeeRankDtoByUserId(dapper, userId, new QuarterInYear(DateTime.Now.AddDays(ReadOnlyValue.SubDay)))));
    }

    /// <summary>
    /// Lấy kết quả xếp loại quý hiện tại của 1 đơn vị theo deptId
    /// </summary>
    /// <param name="deptId"></param>
    /// <returns></returns>
    [HttpGet("GetCurrentByDeptId/{deptId}")]
    public async Task<IActionResult> GetCurrentByDeptId(int deptId)
    {
        return Ok(ResultExtension.GetResult(
            await context.GetQuarterEmployeeRankDtoByDeptId(deptId, new QuarterInYear(DateTime.Now.AddDays(ReadOnlyValue.SubDay)))));
    }

    /// <summary>
    /// Lấy kết quả xếp loại 3 quý trước của 1 đơn vị
    /// </summary>
    /// <param name="deptId"></param>
    /// <returns></returns>
    [HttpGet("Get3QuarterBeforeByDeptId/{deptId}")]
    public async Task<IActionResult> Get3QuarterBeforeByDeptId(int deptId)
    {
        //QuarterInYear q1 = QuarterInYear.After(DateTime.Now.AddDays(ReadOnlyValue.SubDay));
        //QuarterInYear q2 = QuarterInYear.After(q1);
        //QuarterInYear q3 = QuarterInYear.After(q2);
        //var list3 = await dapper.GetByQueryAsync<QuarterEmployeeRankDto>(SqlQueryString.SelectTop3QuarterEmployeeRank(deptId, q1, q2, q3));

        //var result = list3?.DistinctBy(x => x.EmployeeId)
        //                                                .Select(x => new Result3QuarterEmployeeRankDto
        //                                                {
        //                                                    EmployeeId = x.EmployeeId,
        //                                                    EmployeeName = x.EmployeeName!,
        //                                                    TotalReward = GetTotalReward(list3, x.EmployeeId)
        //                                                });
        //return Ok(ResultExtension.GetResult(result));
        return NoContent();
    }

    [HttpGet("Get3QuarterBeforeByEmployeeId/{employeeId}")]
    public async Task<IActionResult> Get3QuarterBeforeByEmployeeId(int employeeId)
    {
        QuarterInYear q1 = QuarterInYear.After(DateTime.Now.AddDays(ReadOnlyValue.SubDay));
        QuarterInYear q2 = QuarterInYear.After(q1);
        QuarterInYear q3 = QuarterInYear.After(q2);
        var list3 = await dapper.GetByQueryAsync<QuarterEmployeeRankDto>(SqlQueryString.SelectTop3QuarterEmployeeRank(employeeId, q1, q2, q3));

        return Ok(ResultExtension.GetResult(list3?.DistinctBy(x => x.EmployeeId)
                                            .Select(x => new Result3QuarterEmployeeRankDto
                                            {
                                                EmployeeId = x.EmployeeId,
                                                EmployeeName = x.EmployeeName!,
                                                TotalReward = GetTotalReward(list3, x.EmployeeId)
                                            }).FirstOrDefault())
                );
    }

    // GET: api/QuarterEmployeeRanks/5
    [HttpGet(ActionBase.GetOne + "/{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        return Ok(ResultExtension.GetResult(await context.QuarterEmployeeRanks.Where(x => x.QuarterEmployeeRankId == id).Include(x => x.Employee)
            .Select(x => x.ToDto()).FirstOrDefaultAsync()));
    }

    // PUT: api/QuarterEmployeeRanks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(ActionBase.Update + "/{id}")]
    public async Task<IActionResult> Update(int id, QuarterEmployeeRankDto dto)
    {
        if (id != dto.QuarterEmployeeRankId)
        {
            return Ok(Result<bool>.Failure(Message.Notfound));
        }

        try
        {
            var quarterEmployeeRank = await context.QuarterEmployeeRanks.FindAsync(id);
            if (quarterEmployeeRank is not null)
            {
                quarterEmployeeRank = dto.ToEntity(quarterEmployeeRank);
                context.Entry(quarterEmployeeRank).State = EntityState.Modified;
                return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuarterEmployeeRankExists(id))
            {
                return Ok(Result<bool>.Failure(Message.Notfound));
            }
            else
            {
                throw;
            }
        }

        return Ok(Result<bool>.Failure(Message.Notfound));
    }

    // POST: api/QuarterEmployeeRanks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost(ActionBase.Create)]
    public async Task<IActionResult> Create(QuarterEmployeeRankDto dto)
    {
        try
        {
            context.QuarterEmployeeRanks.Add(dto.ToEntity());
            return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
        }
        catch (DbUpdateException)
        {
            if (QuarterEmployeeRankExists(dto.QuarterEmployeeRankId))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }
    }

    // DELETE: api/QuarterEmployeeRanks/5
    [HttpDelete(ActionBase.Delete + "/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var quarterEmployeeRank = await context.QuarterEmployeeRanks.FindAsync(id);
        if (quarterEmployeeRank != null)
        {
            context.QuarterEmployeeRanks.Remove(quarterEmployeeRank);

            return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
        }
        return Ok(Result<bool>.Failure(Message.Notfound));
    }

    #endregion cá nhân

    private static string GetTotalReward(IEnumerable<QuarterEmployeeRankDto> ds3QuyTruoc, int employeeId) =>
       string.Join(", ", ds3QuyTruoc.Where(x => x.EmployeeId == employeeId)
                                   .Select(x => $"quý {x.Quarter}-{x.Year}: {x.RewardName}")
           );

    private bool QuarterEmployeeRankExists(int id)
    {
        return context.QuarterEmployeeRanks.Any(e => e.QuarterEmployeeRankId == id);
    }
}