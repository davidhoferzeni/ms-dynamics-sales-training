
using Microsoft.Extensions.Logging;

public class TimerInfo
{
    public ScheduleStatus? ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }

    public void LogStatus(ILogger logger) {
        logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        if (IsPastDue) {
            logger.LogWarning($"Time was past it's original due date! Please check monitoring resources!");
        }
        logger.LogInformation($"Next timer schedule at: {ScheduleStatus?.Next}");
    }
}

public class ScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}