using CharityFlow.Application.Repositories;
using MediatR;
using CharityFlow.Application.Services;

namespace CharityFlow.Application.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _repo;
    private readonly INotificationService _notificationService;

    public UpdateProjectCommandHandler(IProjectRepository repo, INotificationService notificationService)
    {
        _repo = repo;
        _notificationService = notificationService;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken ct)
    {
        var project = await _repo.GetByIdAsync(request.ProjectId, ct);
        project.AddDonation(request.Amount);
        await _repo.UpdateAsync(project, ct);
    }
} 