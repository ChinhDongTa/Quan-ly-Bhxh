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
                Name = workShiftDto.Name
            };
        }
        else
        {
            workShift.Description = workShiftDto.Description;
            workShift.Name = workShiftDto.Name;
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
            };
        }
        else
        {
            workDay.Date = workDayDto.Date;
            return workDay;
        }
    }

    public static WorkScheduleDto ToDto(this WorkSchedule workSchedule)
    {
        return new WorkScheduleDto
        {
            Id = workSchedule.Id,
            StartDay = workSchedule.StartDay,
            EndDay = workSchedule.EndDay,
            UserId = workSchedule.UserId,
            UpdateAt = workSchedule.UpdateAt,
            InfoUserCreated = $"{workSchedule.User?.Employee?.FirstName ?? ""} {workSchedule.User?.Employee?.LastName ?? ""}",
            WorkDays = [.. workSchedule.WorkDays.Select(wd => new WorkDayDto
            {
                Id = wd.Id,
                Date = wd.Date,
                WorkShiftDtos = wd.WorkShifts?.Select(wsf => new WorkShiftDto
                {
                    Id = wsf.Id,
                    Name = wsf.Name,
                    Description = wsf.Description
                }).ToList()
            })]
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