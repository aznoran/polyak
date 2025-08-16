using MediatR;

namespace CharityFlow.Application.Commands;

public record CreateProjectCommand(string Name, decimal TargetAmount) : IRequest<Guid>; 