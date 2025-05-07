using DataServices.Data;
using DataServices.Entities.Human;

namespace ApiGateway.Helpers;

public static class EventLogHelper {

    public static async Task CreateEventLogAsync(this BhxhDbContext context, ApiUser? user, HttpContext httpContext, string actionName, string description)
    {
        if (user != null)
        {
            var eventLog = new EventLog
            {
                UserId = user.Id,
                CreateTime = DateTime.Now,
                IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
                Browser = httpContext.Request.Headers.UserAgent, // Simplified Substring
                ActionName = actionName,
                Description = description
            };
            context.EventLogs.Add(eventLog);
            await context.SaveChangesAsync();
        }
    }
}