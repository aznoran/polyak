using CharityFlow.Application.Repositories;
using CharityFlow.Domain.Aggregates;
using Microsoft.Extensions.Logging;

namespace CharityFlow.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private static readonly Dictionary<Guid, ProjectAggregate> _projects = new();
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(ILogger<ProjectRepository> logger)
    {
        _logger = logger;
    }

    static ProjectRepository()
    {
        // Создаем тестовые проекты с правильными ID
        var project1Id = Guid.NewGuid();
        var project2Id = Guid.NewGuid();
        var project3Id = Guid.NewGuid();
        
        _projects[project1Id] = new ProjectAggregate(project1Id, "Дом для бездомных животных", 50000m);
        _projects[project2Id] = new ProjectAggregate(project2Id, "Помощь детям-сиротам", 120000m);
        _projects[project3Id] = new ProjectAggregate(project3Id, "Озеленение города", 80000m);
        
        // Логируем созданные проекты (в продакшене это можно убрать)
        Console.WriteLine($"Создано {_projects.Count} тестовых проектов:");
        foreach (var project in _projects.Values)
        {
            Console.WriteLine($"  ID: {project.Id}, Название: {project.Name}");
        }
    }

    public Task<ProjectAggregate?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (_projects.TryGetValue(id, out var project))
        {
            _logger.LogInformation("Проект найден: ID={ProjectId}, Название={ProjectName}", id, project.Name);
            return Task.FromResult(project);
        }

        _logger.LogWarning("Проект не найден: ID={ProjectId}", id);
        return Task.FromResult<ProjectAggregate?>(null);
    }

    public Task<IEnumerable<ProjectAggregate>> GetAllAsync(CancellationToken ct)
    {
        var projects = _projects.Values.AsEnumerable();
        _logger.LogInformation("Получено {Count} проектов", projects.Count());
        return Task.FromResult(projects);
    }

    public Task<IEnumerable<Guid>> GetAllIdsAsync(CancellationToken ct)
    {
        var ids = _projects.Keys.AsEnumerable();
        _logger.LogInformation("Получено {Count} ID проектов", ids.Count());
        return Task.FromResult(ids);
    }

    public Task CreateAsync(ProjectAggregate project, CancellationToken ct)
    {
        _projects[project.Id] = project;
        _logger.LogInformation("Создан новый проект: ID={ProjectId}, Название={ProjectName}", project.Id, project.Name);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ProjectAggregate project, CancellationToken ct)
    {
        _projects[project.Id] = project;
        _logger.LogInformation("Обновлен проект: ID={ProjectId}, Название={ProjectName}, Текущая сумма={CurrentAmount}", 
            project.Id, project.Name, project.CurrentAmount);
        return Task.CompletedTask;
    }
} 