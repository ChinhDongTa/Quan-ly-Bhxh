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
public class QuarterDepartmentRanksController(BhxhDbContext context, IGenericDapper dapper) : ControllerBase {

    // GET: QuarterDepartmentRanks
    [HttpGet("GetByQuarter/{q}/{year}")]
    public async Task<IActionResult> GetByQuarter(byte q, int year)
    {
        var quarter = new QuarterInYear(q, year);
        return Ok(ResultExtension.GetResult(await context.QuarterDepartmentRanks
            .Where(x => x.Quarter == q && x.Year == year)
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
    public async Task<IActionResult> Create(QuarterDepartmentRank quarterDepartmentRank)
    {
        context.QuarterDepartmentRanks.Add(quarterDepartmentRank);
        try
        {
            return Ok(Result<bool>.BoolResult(await context.SaveChangesAsync() > 0));
        }
        catch (DbUpdateException)
        {
            if (QuarterDepartmentRankExists(quarterDepartmentRank.Id))
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