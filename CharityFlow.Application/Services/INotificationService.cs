namespace CharityFlow.Application.Services;

public interface INotificationService
{
    Task SendMilestoneEmailAsync(Guid projectId, string milestoneType);
    Task SendMilestoneSmsAsync(Guid projectId, string milestoneType);
} 