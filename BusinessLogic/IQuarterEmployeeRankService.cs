using DataServices.Entities.Human;
using Dtos.Human;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BusinessLogic;

public interface IQuarterEmployeeRankService {
    Task<IEnumerable<QuarterEmployeeRank>> GetAllAsync();
    Task<IEnumerable<QuarterEmployeeRankDto>> GetByQuarterAsync(int deptId, byte quarter, int year);
    Task<QuarterEmployeeRankDto?> GetCurrentByUserIdAsync(string userId);
    Task<QuarterEmployeeRankDto?> GetCurrentByDeptIdAsync(int deptId);
    Task<Result3QuarterEmployeeRankDto?> Get3QuarterBeforeByEmployeeIdAsync(int employeeId);
    Task<QuarterEmployeeRankDto?> GetOneAsync(int id);
    Task<bool> UpdateAsync(int id, QuarterEmployeeRankDto dto, ClaimsPrincipal user, HttpContext httpContext);
    Task<bool> CreateAsync(QuarterEmployeeRankDto dto);
    Task<bool> DeleteAsync(int id);
}

