using CharityFlow.Application.Repositories;
using CharityFlow.Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CharityFlow.Application.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<CreateProjectCommandHandler> _logger;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        ILogger<CreateProjectCommandHandler> logger)
    {
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        try
        {
            // Валидация входных данных
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Название проекта не может быть пустым", nameof(request.Name));
            }

            if (request.TargetAmount <= 0)
            {
                throw new ArgumentException("Целевая сумма должна быть больше нуля", nameof(request.TargetAmount));
            }

            // Создание нового проекта
            var projectId = Guid.NewGuid();
            var project = new ProjectAggregate(projectId, request.Name, request.TargetAmount);

            // Сохранение проекта в репозитории
            await _projectRepository.CreateAsync(project, ct);

            _logger.LogInformation(
                "Создан новый проект: ID={ProjectId}, Название={ProjectName}, Целевая сумма={TargetAmount}",
                projectId, request.Name, request.TargetAmount);

            return projectId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Ошибка при создании проекта: Название={ProjectName}, Целевая сумма={TargetAmount}",
                request.Name, request.TargetAmount);
            throw;
        }
    }
} 