using ApiGateway.Helpers;
using DataServices.Data;
using DataServices.Entities.Human;
using DataTranfer.Mapping;
using DefaultValue;
using DongTa.ResponseMessage;
using DongTa.ResponseResult;
using Dtos.Human;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Controllers.Human;

[Route("[controller]")]
[ApiController]
public class WorkSchedulesController(BhxhDbContext context, UserManager<ApiUser> userManager) : ControllerBase {
    private readonly BhxhDbContext context = context;
    private readonly UserManager<ApiUser> userManager = userManager;

    /// <summary>
    /// Lấy danh sách lịch làm việc theo ngày
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [HttpGet("GetByDate/{date}")]
    public async Task<IActionResult> GetByDate(DateOnly date)
    {
        var workScheduleDto = await context.WorkSchedules
            .Include(ws => ws.WorkDays)
                .ThenInclude(wd => wd.WorkShifts)
            .Include(ws => ws.User)
                .ThenInclude(u => u!.Employee)
            .Where(ws => ws.StartDay <= date && ws.EndDay >= date)
            .Select(ws => new WorkScheduleDto
            {
                Id = ws.Id,
                StartDay = ws.StartDay,
                EndDay = ws.EndDay,
                UserId = ws.UserId,
                InforUserCreated = $"{ws.User!.Employee!.FirstName} {ws.User.Employee.LastName}", // Assuming Employee is a navigation property
                WorkDays = ws.WorkDays.Select(wd => new WorkDayDto
                {
                    Id = wd.Id,
                    Date = wd.Date,
                    WorkShiftDtos = wd.WorkShifts!.Select(wsf => new WorkShiftDto
                    {
                        Id = wsf.Id,
                        Name = wsf.Name,
                        Description = wsf.Description
                    }).ToList()
                }).ToList()
            }).FirstOrDefaultAsync();

        return Ok(ResultExtension.GetResult(workScheduleDto));
    }

    [HttpGet("GetOne/{Id}")]
    public async Task<IActionResult> GetOne(int Id)
    {
        var workScheduleDto = await context.WorkSchedules
           .Include(ws => ws.WorkDays)
               .ThenInclude(wd => wd.WorkShifts)
           .Include(ws => ws.User)
               .ThenInclude(u => u!.Employee).
               Where(ws => ws.Id == Id)
           .Select(ws => new WorkScheduleDto
           {
               Id = ws.Id,
               StartDay = ws.StartDay,
               EndDay = ws.EndDay,
               UserId = ws.UserId,
               InforUserCreated = $"{ws.User!.Employee!.FirstName} {ws.User.Employee.LastName}", // Assuming Employee is a navigation property
               WorkDays = ws.WorkDays.Select(wd => new WorkDayDto
               {
                   Id = wd.Id,
                   Date = wd.Date,
                   WorkShiftDtos = wd.WorkShifts!.Select(wsf => new WorkShiftDto
                   {
                       Id = wsf.Id,
                       Name = wsf.Name,
                       Description = wsf.Description
                   }).ToList()
               }).ToList()
           }).FirstOrDefaultAsync(x => x.Id == Id);
        return Ok(ResultExtension.GetResult(workScheduleDto));
    }

    [HttpPut("UpdateList")]
    [Authorize]
    public async Task<IActionResult> UpdateList([FromBody] List<WorkShiftDto> list)
    {
        foreach (var item in list)
        {
            var workShift = await context.WorkShifts
                .FirstOrDefaultAsync(x => x.Id == item.Id);
            workShift = item.ToEntity(workShift);
            context.Entry(workShift).State = EntityState.Modified;
        }
        if (await context.SaveChangesAsync() > 0)
        {
            await context.CreateEventLogAsync(await userManager.GetUserAsync(User), HttpContext, "UpdateList", "Cập nhật lịch làm việc");
            return Ok(Result<bool>.Success(InfoMessage.Success));
        }
        return Ok(Result<bool>.Failure("Cập nhật lịch làm việc thất bại"));
    }

    [HttpGet("CreateNextWeek/{userId}/{dateOnly}")]
    [Authorize]
    public async Task<IActionResult> CreateNextWeek(string userId, DateOnly dateOnly)
    {
        var apiUser = await userManager.GetUserAsync(User);
        if (apiUser == null || apiUser.Id != userId) // Added null check for apiUser
        {
            return Ok(Result<bool>.Failure(new Message(LevelMessage.Error, "User không hợp lệ")));
        }
        var workScheduleId = await context.InitWorkSchedule(userId, dateOnly);
        await context.CreateEventLogAsync(apiUser, HttpContext, "CreateNextWeek", "Tạo mới lịch làm việc");
        return await GetOne(workScheduleId);
    }
}