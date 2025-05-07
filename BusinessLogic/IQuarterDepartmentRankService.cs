using Dtos.Human;

namespace BusinessLogic;

public interface IQuarterDepartmentRankService {

    Task<List<QuarterDepartmentRankDto>> GetByQuarterAsync(byte quarter, int year);

    Task<QuarterDepartmentRankDto?> GetCurrentByUserIdAsync(string userId);

    Task<QuarterDepartmentRankDto?> GetCurrentByDeptIdAsync(int deptId);

    Task<QuarterDepartmentRankDto?> GetOneAsync(int id);

    Task<bool> UpdateAsync(int id, QuarterDepartmentRankDto dto);

    Task<bool> CreateAsync(QuarterDepartmentRankDto dto);

    Task<bool> DeleteAsync(int id);
}