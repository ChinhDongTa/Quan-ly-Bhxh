using DataServices.Data;
using DataServices.Entities.Human;
using DataTranfer.Mapping;
using DefaultValue.ApiRoute;
using DefaultValue;
using DongTa.BaseDapper;
using DongTa.QuarterInYear;
using Dtos.Human;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BusinessLogic;

public class QuarterEmployeeRankService : IQuarterEmployeeRankService {
    private readonly BhxhDbContext context;
    private readonly IGenericDapper dapper;
    private readonly UserManager<ApiUser> userManager;

    public QuarterEmployeeRankService(BhxhDbContext context, IGenericDapper dapper, UserManager<ApiUser> userManager)
    {
        this.context = context;
        this.dapper = dapper;
        this.userManager = userManager;
    }

    public async Task<IEnumerable<QuarterEmployeeRank>> GetAllAsync()
    {
        return await context.QuarterEmployeeRanks.ToListAsync();
    }

    public async Task<IEnumerable<QuarterEmployeeRankDto>> GetByQuarterAsync(int deptId, byte quarter, int year)
    {
        return await context.GetQuarterEmployeeRankDtoByDeptId(deptId, new QuarterInYear { Quarter = quarter, Year = year });
    }

    public async Task<QuarterEmployeeRankDto?> GetCurrentByUserIdAsync(string userId)
    {
        return await context.GetQuarterEmployeeRankDtoByUserId(dapper, userId, new QuarterInYear(DateTime.Now.AddDays(ReadOnlyValue.SubDay)));
    }

    public async Task<QuarterEmployeeRankDto?> GetCurrentByDeptIdAsync(int deptId)
    {
        return await context.GetQuarterEmployeeRankDtoByDeptId(deptId, new QuarterInYear(DateTime.Now.AddDays(ReadOnlyValue.SubDay)));
    }

    public async Task<Result3QuarterEmployeeRankDto?> Get3QuarterBeforeByEmployeeIdAsync(int employeeId)
    {
        QuarterInYear q1 = QuarterInYear.After(DateTime.Now.AddDays(ReadOnlyValue.SubDay));
        QuarterInYear q2 = QuarterInYear.After(q1);
        QuarterInYear q3 = QuarterInYear.After(q2);

        var list3 = await dapper.GetByQueryAsync<QuarterEmployeeRankDto>(SqlQueryString.SelectTop3QuarterEmployeeRank(employeeId, q1, q2, q3));

        return list3?.DistinctBy(x => x.EmployeeId)
            .Select(x => new Result3QuarterEmployeeRankDto
            {
                EmployeeId = x.EmployeeId,
                EmployeeName = x.EmployeeName!,
                TotalReward = GetTotalReward(list3, x.EmployeeId)
            }).FirstOrDefault();
    }

    public async Task<QuarterEmployeeRankDto?> GetOneAsync(int id)
    {
        return await context.QuarterEmployeeRanks
            .Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(int id, QuarterEmployeeRankDto dto, ClaimsPrincipal user, HttpContext httpContext)
    {
        if (id != dto.Id) return false;

        var quarterEmployeeRank = await context.QuarterEmployeeRanks.FindAsync(id);
        if (quarterEmployeeRank == null) return false;

        quarterEmployeeRank = dto.ToEntity(quarterEmployeeRank);
        context.Entry(quarterEmployeeRank).State = EntityState.Modified;

        var success = await context.SaveChangesAsync() > 0;
        if (success)
        {
            await context.CreateEventLogAsync(await userManager.GetUserAsync(user), httpContext, ActionBase.Update, $"Updated QuarterEmployeeRank with Id={id}");
        }

        return success;
    }

    public async Task<bool> CreateAsync(QuarterEmployeeRankDto dto)
    {
        context.QuarterEmployeeRanks.Add(dto.ToEntity());
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var quarterEmployeeRank = await context.QuarterEmployeeRanks.FindAsync(id);
        if (quarterEmployeeRank == null) return false;

        context.QuarterEmployeeRanks.Remove(quarterEmployeeRank);
        return await context.SaveChangesAsync() > 0;
    }

    private static string GetTotalReward(IEnumerable<QuarterEmployeeRankDto> ds3QuyTruoc, int employeeId) =>
        string.Join(", ", ds3QuyTruoc.Where(x => x.EmployeeId == employeeId)
            .Select(x => $"Quarter {x.Quarter}-{x.Year}: {x.RewardName}"));
}
