using CharityFlow.Domain.Aggregates;

namespace CharityFlow.Application.Repositories;

public interface IProjectRepository
{
    Task<ProjectAggregate?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<ProjectAggregate>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<Guid>> GetAllIdsAsync(CancellationToken ct);
    Task CreateAsync(ProjectAggregate project, CancellationToken ct);
    Task UpdateAsync(ProjectAggregate project, CancellationToken ct);
} 