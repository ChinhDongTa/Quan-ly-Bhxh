using DataServices.Entities.Human;
using Dtos.Human;

namespace DataTranfer.Mapping;

public static class WorkScheduleMapper {

    public static WorkShiftDto ToDto(this WorkShift workShift)
    {
        return new()
        {
            Id = workShift.Id,
            Name = workShift.Name,
            Description = workShift.Description,
            WorkDayId = workShift.WorkDayId,
            DateString = workShift.WorkDay?.Date.ToString("dd/MM/yyyy") // Ngày làm việc: Thứ hai 18/04/2025
        };
    }

    public static WorkShift ToEntity(this WorkShiftDto workShiftDto, WorkShift? workShift)
    {
        if (workShift is null)
        {
            return new WorkShift()
            {
                Description = workShiftDto.Description,
                Id = workShiftDto.Id,
                Name = workShiftDto.Name,
                WorkDayId = workShiftDto.WorkDayId
            };
        }
        else
        {
            workShift.Description = workShiftDto.Description;
            workShift.Name = workShiftDto.Name;
            workShift.WorkDayId = workShiftDto.WorkDayId;
            return workShift;
        }
    }

    public static WorkDayDto ToDto(this WorkDay workDay)
    {
        return new()
        {
            Id = workDay.Id,
            Date = workDay.Date,
            WorkShiftDtos = workDay.WorkShifts?.Select(x => x.ToDto()).ToList(),
            WorkScheduleId = workDay.WorkScheduleId,
        };
    }

    public static WorkDay ToEntity(this WorkDayDto workDayDto, WorkDay? workDay = null)
    {
        if (workDay == null)
        {
            return new()
            {
                Id = workDayDto.Id,
                Date = workDayDto.Date,
                WorkScheduleId = workDayDto.WorkScheduleId
            };
        }
        else
        {
            workDay.Date = workDayDto.Date;
            workDay.WorkScheduleId = workDayDto.WorkScheduleId;
            return workDay;
        }
    }

    public static WorkScheduleDto ToDto(this WorkSchedule workSchedule)
    {
        return new()
        {
            Id = workSchedule.Id,
            StartDay = workSchedule.StartDay,
            EndDay = workSchedule.EndDay,
            UserId = workSchedule.UserId,
            WorkDays = workSchedule.WorkDays?.Select(x => x.ToDto()).ToList() ?? [], // Ensure non-null assignment
            InforUserCreated = $"{workSchedule.User?.Employee?.FirstName} {workSchedule.User?.Employee?.LastName}", // Assuming Employee is a navigation property
            UpdateAt = workSchedule.UpdateAt
        };
    }

    public static WorkSchedule ToEntity(this WorkScheduleDto workScheduleDto, WorkSchedule? workSchedule = null)
    {
        if (workSchedule == null)
        {
            return new()
            {
                Id = workScheduleDto.Id,
                StartDay = workScheduleDto.StartDay,
                EndDay = workScheduleDto.EndDay,
                UserId = workScheduleDto.UserId,
                UpdateAt = DateTime.Now,
            };
        }
        else
        {
            workSchedule.StartDay = workScheduleDto.StartDay;
            workSchedule.EndDay = workScheduleDto.EndDay;
            workSchedule.UserId = workScheduleDto.UserId;
            workSchedule.UpdateAt = DateTime.Now;
            return workSchedule;
        }
    }
}