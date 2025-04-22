using ApiGateway.Helpers;
using DataServices.Data;
using DataTranfer.Mapping;
using DongTa.ResponseResult;
using Dtos.Human;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Controllers.Human {

    [Route("[controller]")]
    [ApiController]
    public class WorkSchedulesController(BhxhDbContext context) : ControllerBase {

        /// <summary>
        /// Lấy danh sách lịch làm việc hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrent")]
        public async Task<IActionResult> GetCurrent()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var workSchedule = await context.WorkSchedules
                .Include(x => x.WorkDays)
                .ThenInclude(x => x.WorkShifts)
                .FirstOrDefaultAsync(x => x.StartDay <= today && x.EndDay >= today);
            return Ok(workSchedule?.ToDto());
        }

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
              .FirstOrDefaultAsync(x => x.StartDay <= date && x.EndDay >= date);
            return Ok(workSchedule?.ToDto());
        }

        [HttpGet("GetOne/{Id}")]
        public async Task<IActionResult> GetOne(int Id)
        {
            var workSchedule = await context.WorkSchedules
                .Include(x => x.WorkDays)
                .ThenInclude(x => x.WorkShifts)
                .Include(x => x.User)
                .ThenInclude(x => x!.Employee)
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
            return Ok(ResultExtension.GetResult(await context.SaveChangesAsync() > 0));
        }

        [HttpGet("CreateNextWeek/{userId}/{dateOnly}")]
        public async Task<IActionResult> CreateNextWeek(string userId, DateOnly dateOnly)
        {
            //var userId = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            await context.InitWorkSchedule(userId, dateOnly);
            return Ok(true);
        }
    }
}