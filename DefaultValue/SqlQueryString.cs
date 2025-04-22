using DongTa.QuarterInYear;

namespace DefaultValue;

public static class SqlQueryString {

    #region EmployeeDb

    private static readonly string SelectDepartmentDto = $"""
        SELECT
          d.Id,
          d.Name,
          d.ShortName,
          d.Score,
          d.IsActivity,
          d.Phone,
          d.Email,
          d.SortOrder,
          d.LevelId,
          Levels.Name AS LevelName
        FROM
          Departments AS d
          INNER JOIN Employees ON d.Id = Employees.DeptId
          INNER JOIN Levels ON d.LevelId = Levels.Id
        """;

    public static string SelectDepartmentDtoByUserId(string userId) => $"""
        {SelectDepartmentDto}
        INNER JOIN AspNetUsers AS a ON a.EmployeeId = Employees.Id
        WHERE
          a.Id = '{userId}'
        """;

    public static string SelectDepartmentDtoByEmployeeId(int employeeId) => $"""
        {SelectDepartmentDto}
        WHERE
          Employees.Id = {employeeId}
        """;

    public static string SelectEmployeeDto { get; } = $"""
        SELECT
          e.Id,
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
        """;

    public static string GetDeptHeadByUserId(string userId)
    {
        return $"""
            DECLARE @deptId INT
            SET @deptId = ({SelectDeptIdByUserId(userId)})
            {SelectEmployeeDto}
            FROM
              Departments as d
              INNER JOIN Employees AS e ON d.Id = e.DeptId
              INNER JOIN Positions as p ON e.PostId = p.Id
              INNER JOIN SalaryCoefficients as s ON e.SalaryCoefficientId = s.Id
            WHERE
              e.IsQuitJob = 0
              AND e.DeptId = @deptId
              AND e.SortOrder = (SELECT MIN(Employees.SortOrder)
                                 FROM Employees
                                 WHERE Employees.IsQuitJob = 0 AND Employees.DeptId = @deptId)
            """;
    }

    public static string SelectDeptIdByUserId(string userId) => $"""
            SELECT
              Departments.Id
            FROM
              Departments
              INNER JOIN Employees ON Departments.Id = Employees.DeptId
              INNER JOIN AspNetUsers ON AspNetUsers.EmployeeId = Employees.Id
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
              Employees as e
              INNER JOIN QuarterEmployeeRanks AS q ON e.Id = q.EmployeeId
              INNER JOIN Rewards AS r ON r.Id = q.RewardId
            where
              e.Id = {employeeId}
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
            FROM
              Departments as d
              INNER JOIN Employees AS e ON d.Id = e.DeptId
              INNER JOIN Positions as p ON e.PostId = p.Id
              INNER JOIN SalaryCoefficients as s ON e.SalaryCoefficientId = s.Id
              INNER JOIN AspNetUsers ON e.Id = AspNetUsers.EmployeeId
            WHERE
              AspNetUsers.Id = '{userId}'
            """;
    }

    public static string SelectDepartmentRankDto { get; } = $"""
        SELECT
          q.Id,
          q.DeptId,
          Departments.Name AS DeptName,
          q.RewardId,
          r.Name AS RewardName,
          q.Quarter,
          q.Year,
          q.SelfScore,
          q.ResultScore,
          Departments.Score AS BaseCore,
          q.Note
        FROM
          Department
          INNER JOIN QuarterDepartmentRanks as q ON Departments.Id = q.DeptId
          INNER JOIN Rewards as r ON q.RewardId = r.Id
        """;

    #endregion EmployeeDb
}