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

namespace ApiGateway.Controllers.Human {

    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
            var workSchedule = await context.WorkSchedules
                .Include(x => x.WorkDays)
                .ThenInclude(x => x.WorkShifts)
                .Include(x => x.User)
                .ThenInclude(x => x!.Employee).AsNoTracking()
                .FirstOrDefaultAsync(x => x.StartDay <= date && x.EndDay >= date);
            return Ok(ResultExtension.GetResult(workSchedule?.ToDto()));
        }

        [HttpGet("GetOne/{Id}")]
        public async Task<IActionResult> GetOne(int Id)
        {
            var workSchedule = await context.WorkSchedules
                .Include(x => x.WorkDays)
                .ThenInclude(x => x.WorkShifts)
                .Include(x => x.User)
                .ThenInclude(x => x!.Employee).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id);
            return Ok(ResultExtension.GetResult(workSchedule?.ToDto()));
        }

        [HttpPut("UpdateList")]
        public async Task<IActionResult> UpdateList([FromBody] List<WorkShiftDto> list)
        {
            foreach (var item in list)
            {
                var workShift = await context.WorkShifts
                    .FirstOrDefaultAsync(x => x.Id == item.Id);
                workShift = item.ToEntity(workShift);

                context.Entry(workShift).State = EntityState.Modified;
            }
            var success = await context.SaveChangesAsync() > 0;
            if (success)
            {
                await CreateEventLogAsync("UpdateList", "Cập nhật lịch làm việc");
                return Ok(Result<bool>.Success(InfoMessage.Success));
            }

            return Ok(Result<bool>.Failure("Cập nhật lịch làm việc thất bại"));
        }

        [HttpGet("CreateNextWeek/{userId}/{dateOnly}")]
        public async Task<IActionResult> CreateNextWeek(string userId, DateOnly dateOnly)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(Result<bool>.Failure(new Message(LevelMessage.Error, "Không tìm thấy người dùng")));
            }

            await context.InitWorkSchedule(userId, dateOnly);
            await CreateEventLogAsync("CreateNextWeek", "Tạo mới lịch làm việc");
            var date = WorkScheduleHelper.GetNextMonday(dateOnly);

            return await GetByDate(date);
        }

        private async Task CreateEventLogAsync(string actionName, string description)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var eventLog = new EventLog
                {
                    UserId = user.Id,
                    CreateTime = DateTime.Now,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Browser = HttpContext.Request.Headers.UserAgent.ToString(),
                    ActionName = actionName,
                    Description = description
                };
                context.EventLogs.Add(eventLog);
                await context.SaveChangesAsync();
            }
        }
    }
}