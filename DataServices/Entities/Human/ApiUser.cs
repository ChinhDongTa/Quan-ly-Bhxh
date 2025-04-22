using Microsoft.AspNetCore.Identity;

namespace DataServices.Entities.Human;

public class ApiUser : IdentityUser {
    public int EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }
    public virtual ICollection<EventLog>? EventLogs { get; set; }
    public virtual ICollection<WorkSchedule>? WorkSchedules { get; set; }
}