using MediatR;

namespace CharityFlow.Application.Commands;

public record UpdateProjectCommand(Guid ProjectId, decimal Amount) : IRequest; 