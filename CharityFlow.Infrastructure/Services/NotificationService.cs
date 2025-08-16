using CharityFlow.Application.Services;
using Microsoft.Extensions.Logging;

namespace CharityFlow.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    
    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }
    
    public Task SendMilestoneEmailAsync(Guid projectId, string milestoneType)
    {
        _logger.LogInformation("EMAIL ОТПРАВЛЕН: Проект {ProjectId} достиг этапа {MilestoneType}", projectId, milestoneType);
        return Task.CompletedTask;
    }
    
    public Task SendMilestoneSmsAsync(Guid projectId, string milestoneType)
    {
        _logger.LogInformation("SMS ОТПРАВЛЕН: Проект {ProjectId} достиг этапа {MilestoneType}", projectId, milestoneType);
        return Task.CompletedTask;
    }
} 