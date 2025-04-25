using DataServices.Data;
using DataServices.Entities.Human;

namespace ApiGateway.Helpers;

public static class WorkScheduleHelper {

    /// <summary>
    /// Khởi tạo lịch làm việc cho người dùng
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userId">mã người tạo</param>
    /// <param name="date">ngày hiện tại hoặc ngày bất kỳ muốn tạo lịch cho tuần tiếp theo</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task InitWorkSchedule(this BhxhDbContext context, string userId, DateOnly? date)
    {
        // Fix for CS0201: Assign the result of the null-coalescing operation to nextMonday
        date ??= DateOnly.FromDateTime(DateTime.Now);
        var nextMonday = GetNextMonday(date.Value);
        // Nếu đã có lịch làm việc cho tuần này thì không tạo mới
        if (context.WorkSchedules.Any(x => x.EndDay >= nextMonday))
        {
            return;
        }
        WorkSchedule workSchedule = new()
        {
            StartDay = nextMonday,
            EndDay = nextMonday.AddDays(6),
            WorkDays = [], // Initialize WorkDays to avoid null reference
            UserId = userId,
            UpdateAt = DateTime.Now
        };

        context.WorkSchedules.Add(workSchedule);
        await context.SaveChangesAsync();
        if (workSchedule.Id == 0)
        {
            throw new Exception("Lỗi tạo lịch làm việc");
        }
        // Tạo các WorkDay cho lịch làm việc
        var monday = new WorkDay()
        {
            Date = workSchedule.StartDay,
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(monday);
        var tue = new WorkDay()
        {
            Date = workSchedule.StartDay.AddDays(1),
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(tue);
        var wed = new WorkDay()
        {
            Date = workSchedule.StartDay.AddDays(2),
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(wed);
        var thu = new WorkDay()
        {
            Date = workSchedule.StartDay.AddDays(3),
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(thu);
        var fri = new WorkDay()
        {
            Date = workSchedule.StartDay.AddDays(4),
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(fri);
        var sat = new WorkDay()
        {
            Date = workSchedule.StartDay.AddDays(5),
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(sat);
        var sun = new WorkDay()
        {
            Date = workSchedule.StartDay.AddDays(6),
            WorkScheduleId = workSchedule.Id
        };
        workSchedule.WorkDays.Add(sun);
        await context.WorkDays.AddRangeAsync(workSchedule.WorkDays);
        var ok = await context.SaveChangesAsync();
        if (ok == 0)
        {
            throw new Exception("Lỗi tạo lịch làm việc");
        }
        // Tạo các WorkShift cho lịch làm việc
        foreach (var workDay in workSchedule.WorkDays)
        {
            // Ensure WorkShifts is initialized before adding
            workDay.WorkShifts = [];

            var morning = new WorkShift()
            {
                WorkDayId = workDay.Id,
                Name = "Sáng",
                Description = "-Làm việc",
            };

            workDay.WorkShifts.Add(morning);
            var afternoon = new WorkShift()
            {
                WorkDayId = workDay.Id,
                Name = "Chiều",
                Description = "-Làm việc",
            };
            workDay.WorkShifts.Add(afternoon);
        }
        await context.SaveChangesAsync();
    }

    public static DateOnly GetNextMonday(DateOnly date)
    {
        int daysToAdd = (7 - (int)date.DayOfWeek + 1) % 7;
        if (daysToAdd == 0)
        {
            daysToAdd = 7; // Nếu ngày đã là thứ hai, thêm 7 ngày để lấy thứ hai tiếp theo
        }
        return date.AddDays(daysToAdd);
    }
}