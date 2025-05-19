using ApiGateway.Controllers.File.Models;
using DataServices.Data;
using DataTranfer.Mapping;
using DongTa.TypeExtension;
using Dtos.Human;
using Dtos.Parameter;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Controllers.File.Rdlcs;

internal class DataSourceConverter {

    public static async Task<List<TongHopQuyLine>?> GetTongHopQuyDataSource(BhxhDbContext context, TongHopQuyPara thamSo)
    {
        try
        {
            // Lấy danh sách phòng ban
            List<QuarterDepartmentRankDto>? xepLoaiDonVi = await GetDepartmentRanksAsync(context, thamSo);

            if (xepLoaiDonVi is not null)
            {
                var deptIds = xepLoaiDonVi.Select(d => d.DeptId).ToList();
                List<QuarterEmployeeRankDto> allEmployeeRanks = await GetQuarterEmployeeRankDtoBy(context, thamSo, deptIds);

                // Nhóm nhân viên theo DeptId
                var employeeGroups = allEmployeeRanks.GroupBy(e => e.DeptId).ToDictionary(g => g.Key, g => g.ToList());

                var list = new List<TongHopQuyLine>();
                int sttDept = 1;

                foreach (var item in xepLoaiDonVi)
                {
                    // Thêm dòng đơn vị
                    TongHopQuyLine deptLine = new()
                    {
                        Roman = sttDept++.ToRomanNumeral(),
                        DeptName = item.DeptName,
                        SelfClassificationDept = item.RewardName,
                        Group = GetGroupByRewardName(item.RewardName!)
                    };

                    if (item.DeptId != 1) // Trừ ban giám đốc ra
                    {
                        if (item.SelfScore.HasValue)
                        {
                            deptLine.SelfScoreDept = CalculateScore(item.SelfScore, item.BaseCore);
                        }
                        if (item.ResultScore.HasValue)
                        {
                            deptLine.ResultScoreDept = CalculateScore(item.ResultScore, item.BaseCore);
                        }
                    }

                    // Lấy danh sách nhân viên của phòng ban hiện tại
                    if (employeeGroups.TryGetValue(item.DeptId, out var xepLoaiCaNhans))
                    {
                        deptLine.SummaryDept = GetSummaryDept(xepLoaiCaNhans, item.DeptId);
                        list.Add(deptLine);

                        // Thêm dòng cá nhân
                        int sttCaNhan = 1;
                        foreach (var caNhan in xepLoaiCaNhans)
                        {
                            var caNhanLine = CreateEmployeeLine(caNhan, deptLine, sttCaNhan++);
                            list.Add(caNhanLine);
                        }
                    }
                }
                return list;
            }
            return null;
        }
        catch (Exception ex)
        {
            // Ghi log lỗi
            Console.WriteLine($"Error in GetTongHopQuyDataSource: {ex.Message}");
            throw;
        }
    }

    private static string CalculateScore(int? score, int baseCore)
    {
        return score.HasValue
            ? $"{score}/{baseCore} = {((double)score.Value / baseCore).ToPercentString(1)}"
            : string.Empty;
    }

    private static async Task<List<QuarterEmployeeRankDto>> GetQuarterEmployeeRankDtoBy(BhxhDbContext context, TongHopQuyPara thamSo, List<int> deptIds)
    {
        var allEmployeeRanks = await context.QuarterEmployeeRanks
            .Include(x => x.Employee).ThenInclude(x => x!.Dept)
            .Where(e => e.Employee != null && e.Employee.Dept != null && deptIds.Contains(e.Employee.Dept.Id) && e.Quarter == thamSo.Quarter && e.Year == thamSo.Year)
            .Include(x => x.Reward)
            .Select(e => new QuarterEmployeeRankDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = $"{e.Employee!.FirstName} {e.Employee.LastName}",
                RewardName = e.Reward!.Name,
                SelfScore = e.SelfScore,
                ResultScore = e.ResultScore,
                Note = e.Note,
                DeptId = e.Employee!.Dept!.Id // Thêm DeptId để nhóm
            })
            .ToListAsync();
        return allEmployeeRanks;
    }

    public static IEnumerable<XepLoaiCaNhanLine>? ToXepLoaiCaNhanLines(IEnumerable<QuarterEmployeeRankDto>? list)
    {
        if (list is not null)
        {
            byte index = 1;
            var result = new List<XepLoaiCaNhanLine>();
            foreach (var item in list)
            {
                XepLoaiCaNhanLine line = new()
                {
                    STT = index++,
                    FullName = item.EmployeeName!,
                    Note = item.Note
                };
                switch (item.RewardId)
                {
                    case 20: line.A = "x"; break;
                    case 21: line.B = "x"; break;
                    case 22: line.C = "x"; break;
                    case 23: line.D = "x"; break;
                    default:
                        break;
                }
                result.Add(line);
            }
            return result;
        }
        return null;
    }

    internal static IEnumerable<TongHopQuyCcVc>? ToTongHopKetQuaCongTacQuy(IEnumerable<QuarterEmployeeRankDto>? list)
    {
        if (list is not null)
        {
            var result = new List<TongHopQuyCcVc>();
            byte stt = 1;
            foreach (var item in list)
            {
                result.Add(new TongHopQuyCcVc()
                {
                    TotalWork = item.TotalWork ?? 0,
                    Note = item.Note,
                    NumWorked = item.NumWorked ?? 0,
                    Reward = item.RewardName!,
                    StaffName = item.EmployeeName!,
                    STT = stt++,
                    SelfReward = GetSelfReward(item.SelfScore)
                });
            }
            return result;
        }
        return null;
    }

    private static TongHopQuyLine CreateEmployeeLine(QuarterEmployeeRankDto caNhan, TongHopQuyLine deptLine, int sttCaNhan)
    {
        var caNhanLine = new TongHopQuyLine
        {
            Roman = deptLine.Roman,
            STT = sttCaNhan,
            FullName = caNhan.EmployeeName,
            DeptName = deptLine.DeptName,
            Note = caNhan.Note,
            SelfScore = caNhan.SelfScore != null ? caNhan.SelfScore.Value.ToString() : string.Empty,
            ResultScore = caNhan.ResultScore != null ? caNhan.ResultScore.Value.ToString() : string.Empty,
        };

        switch (caNhan.RewardName)
        {
            case "A":
                caNhanLine.A = 'A';
                caNhanLine.Personal = "1,3";
                break;

            case "B":
                caNhanLine.B = 'B';
                caNhanLine.Personal = "1,2";
                break;

            case "C":
                caNhanLine.C = 'C';
                caNhanLine.Personal = "0,7";
                break;

            case "D":
                caNhanLine.D = 'D';
                caNhanLine.Personal = "0";
                break;

            default:
                break;
        }
        return caNhanLine;
    }

    private static string GetGroupByRewardName(string rewardName) => rewardName switch
    {
        "A" => "1,0",
        "B" => "0,97",
        "C" => "0,94",
        "D" => "0,9",
        _ => string.Empty
    };

    private static async Task<List<QuarterDepartmentRankDto>> GetDepartmentRanksAsync(BhxhDbContext context, TongHopQuyPara thamSo)
    {
        return await context.QuarterDepartmentRanks
            .Where(x => x.Quarter == thamSo.Quarter && x.Year == thamSo.Year)
            .Include(x => x.Dept)
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    private static string? GetSummaryDept(IEnumerable<QuarterEmployeeRankDto> xepLoaiCaNhan, int deptId)
    {
        if (deptId != 1)//trừ ban giám đốc
        {
            string result = string.Empty;
            int A = xepLoaiCaNhan.Count(x => x.RewardId == 22);
            int B = xepLoaiCaNhan.Count(x => x.RewardId == 23);
            int C = xepLoaiCaNhan.Count(x => x.RewardId == 24);
            int D = xepLoaiCaNhan.Count(x => x.RewardId == 25);
            if (A > 0)
                result = $"{A}A";
            if (B > 0)
                result = $"{result}, {B}B";
            if (C > 0)
                result = $"{result}, {C}C";
            if (D > 0)
                result = $"{result}, {D}D";
            return result;
        }
        return null;
    }

    private static string GetSelfReward(int? selfScore) => selfScore.HasValue
            ? selfScore switch
            {
                >= 90 => "A",
                < 90 and >= 80 => "B",
                < 80 and > 50 => "C",
                _ => "D",
            }
            : string.Empty;
}