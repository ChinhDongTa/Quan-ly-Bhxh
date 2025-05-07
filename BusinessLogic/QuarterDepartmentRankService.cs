using DataServices.Data;
using DataTranfer.Mapping;
using DongTa.BaseDapper;
using Dtos.Human;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class QuarterDepartmentRankService : IQuarterDepartmentRankService {
    private readonly BhxhDbContext context;
    private readonly IGenericDapper dapper;

    public QuarterDepartmentRankService(BhxhDbContext context, IGenericDapper dapper)
    {
        this.context = context;
        this.dapper = dapper;
        this.dapper.DbNameType = DatabaseNameType.Employee;
    }

    public async Task<List<QuarterDepartmentRankDto>> GetByQuarterAsync(byte quarter, int year)
    {
        var list = await context.QuarterDepartmentRanks
            .Where(x => x.Quarter == quarter && x.Year == year)
            .Include(x => x.Dept).Include(x => x.Reward)
            .Select(x => x.ToDto()).ToListAsync();

        var departments = await context.Departments.Where(x => x.IsActive).ToListAsync();

        foreach (var dept in departments)
        {
            if (!list.Any(x => x.DeptId == dept.Id))
            {
                list.Add(new QuarterDepartmentRankDto
                {
                    Id = 0,
                    DeptId = dept.Id,
                    DeptName = dept.Name,
                    RewardId = 0,
                    RewardName = "",
                    Quarter = quarter,
                    Year = year,
                    SelfScore = 0,
                    ResultScore = 0,
                    BaseCore = 0,
                    Note = ""
                });
            }
        }

        return list;
    }

    public async Task<QuarterDepartmentRankDto?> GetCurrentByUserIdAsync(string userId)
    {
        return await context.GetCurrentQuarterDepartmentRankDtoByUserId(dapper, userId);
    }

    public async Task<QuarterDepartmentRankDto?> GetCurrentByDeptIdAsync(int deptId)
    {
        return await context.GetCurrentQuarterDepartmentRankDtoByDeptId(deptId);
    }

    public async Task<QuarterDepartmentRankDto?> GetOneAsync(int id)
    {
        var entity = await context.QuarterDepartmentRanks.FindAsync(id);
        return entity?.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, QuarterDepartmentRankDto dto)
    {
        if (id != dto.Id) return false;

        var oldEntity = await context.QuarterDepartmentRanks.FindAsync(id);
        if (oldEntity == null) return false;

        oldEntity = dto.ToEntity(oldEntity);
        context.Entry(oldEntity).State = EntityState.Modified;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateAsync(QuarterDepartmentRankDto dto)
    {
        if (dto.RewardId == 0) dto.RewardId = 20;

        context.QuarterDepartmentRanks.Add(dto.ToEntity());
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.QuarterDepartmentRanks.FindAsync(id);
        if (entity == null) return false;

        context.QuarterDepartmentRanks.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
