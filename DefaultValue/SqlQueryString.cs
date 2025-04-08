using DongTa.QuarterInYear;

namespace DefaultValue;

public static class SqlQueryString {

    #region EmployeeDb

    private static readonly string SelectDepartmentDto = $"""
        SELECT
          d.DepartmentId,
          d.Name,
          d.ShortName,
          d.Score,
          d.IsActivity,
          d.Phone,
          d.Email,
          d.SortOrder,
          d.LevelId,
          [Level].Name AS LevelName
        FROM
          Department AS d
          INNER JOIN Employee ON d.DepartmentId = Employee.DeptId
          INNER JOIN [Level] ON d.LevelId = [Level].LevelId
        """;

    public static string SelectDepartmentDtoByUserId(string userId) => $"""
        {SelectDepartmentDto}
        INNER JOIN AspNetUsers AS a ON a.EmployeeId = Employee.EmployeeId
        WHERE
          a.Id = '{userId}'
        """;

    public static string SelectDepartmentDtoByEmployeeId(int employeeId) => $"""
        {SelectDepartmentDto}
        WHERE
          EmployeeId = {employeeId}
        """;

    public static string SelectEmployeeDto { get; } = $"""
        SELECT
          e.EmployeeId,
          e.FirstName,
          e.LastName,
          e.Email,
          e.IdentityCard,
          e.Phone,
          e.Address,
          e.DeptId,
          d.Name AS DeptName,
          e.PostId,
          p.Name AS PositionName,
          e.SalaryCoefficientId,
          s.Description AS Salary,
          e.AccountBank,
          e.Birthdate,
          e.IsQuitJob,
          e.SortOrder,
          e.TelegramId,
          e.Gender
        FROM
          Department as d
          INNER JOIN Employee AS e ON d.DepartmentId = e.DeptId
          INNER JOIN Position as p ON e.PostId = p.PositionId
          INNER JOIN SalaryCoefficient as s ON e.SalaryCoefficientId = s.SalaryCoefficientId
          INNER JOIN AspNetUsers ON e.EmployeeId = AspNetUsers.EmployeeId
        """;

    public static string GetDeptHeadByUserId(string userId)
    {
        return $"""
            DECLARE @deptId INT
            SET @deptId = ({SelectDeptIdByUserId(userId)})
            {SelectEmployeeDto}
            WHERE
              e.IsQuitJob = 0
              AND e.DeptId = @deptId
              AND e.SortOrder = (SELECT MIN(Employee.SortOrder)
                                 FROM Employee
                                 WHERE Employee.IsQuitJob = 0 AND Employee.DeptId = @deptId)
            """;
    }

    public static string SelectDeptIdByUserId(string userId) => $"""
            SELECT
              Department.DepartmentId
            FROM
              Department
              INNER JOIN Employee ON Department.DepartmentId = Employee.DeptId
              INNER JOIN AspNetUsers ON AspNetUsers.EmployeeId = Employee.EmployeeId
            WHERE
              (AspNetUsers.Id = '{userId}')
            """;

    public static string SelectTop3QuarterEmployeeRank(int employeeId, QuarterInYear q1, QuarterInYear q2, QuarterInYear q3) => $"""
            SELECT
              q.EmployeeId,
              e.FirstName + ' ' + e.LastName AS EmployeeName,
              q.Quarter,
              q.Year,
              r.Name AS RewardName
            FROM
              Employee as e
              INNER JOIN QuarterEmployeeRank AS q ON e.EmployeeId = q.EmployeeId
              INNER JOIN Reward AS r ON r.RewardId = q.RewardId
            where
              e.EmployeeId = {employeeId}
              AND (
                    (q.Year = {q1.Year} AND q.Quarter = {q1.Quarter})
                    OR
                    (q.Year = {q2.Year} AND q.Quarter = {q2.Quarter})
                    OR
                    (q.Year = {q3.Year} AND q.Quarter = {q3.Quarter})
                )
            ORDER BY Year, Quarter
            """;

    public static string SelectEmployeeDtoByUserId(string userId)
    {
        return $"""
            {SelectEmployeeDto}
            WHERE
              AspNetUsers.Id = '{userId}'
            """;
    }

    public static string SelectDepartmentRankDto { get; } = $"""
        SELECT
          QuarterDepartmentRank.QuarterDepartmentRankId,
          QuarterDepartmentRank.DeptId,
          Department.Name AS DeptName,
          QuarterDepartmentRank.RewardId,
          Reward.Name AS RewardName,
          QuarterDepartmentRank.Quarter,
          QuarterDepartmentRank.Year,
          QuarterDepartmentRank.SelfScore,
          QuarterDepartmentRank.ResultScore,
          Department.Score AS BaseCore,
          QuarterDepartmentRank.Note
        FROM
          Department
          INNER JOIN QuarterDepartmentRank ON Department.DepartmentId = QuarterDepartmentRank.DeptId
          INNER JOIN Reward ON QuarterDepartmentRank.RewardId = Reward.RewardId
        """;

    #endregion EmployeeDb
}