using CharityFlow.Application.Commands;
using CharityFlow.Application.Repositories;
using CharityFlow.Domain.Aggregates;
using CharityFlow.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CharityFlow.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IProjectRepository _projectRepository;

    public ProjectsController(IMediator mediator, IProjectRepository projectRepository)
    {
        _mediator = mediator;
        _projectRepository = projectRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        try
        {
            var command = new CreateProjectCommand(request.Name, request.TargetAmount);
            var projectId = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(GetProject), new { id = projectId }, new { Id = projectId, Message = "Проект успешно создан" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Внутренняя ошибка сервера при создании проекта" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects(CancellationToken ct)
    {
        var projects = await _projectRepository.GetAllAsync(ct);
        var responses = projects.Select(p => new ProjectResponse(
            p.Id, 
            p.Name, 
            p.TargetAmount, 
            p.CurrentAmount, 
            p.GetProgressPercentage()
        ));
        
        return Ok(responses);
    }

    [HttpGet("ids")]
    public async Task<IActionResult> GetAllProjectIds(CancellationToken ct)
    {
        var ids = await _projectRepository.GetAllIdsAsync(ct);
        return Ok(ids);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(Guid id, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(id, ct);
        if (project == null)
            return NotFound();
        
        var response = new ProjectResponse(
            project.Id, 
            project.Name, 
            project.TargetAmount, 
            project.CurrentAmount, 
            project.GetProgressPercentage()
        );
        
        return Ok(response);
    }

    [HttpPost("donate")]
    public async Task<IActionResult> Donate([FromBody] DonateRequest request)
    {
        try
        {
            // Проверяем существование проекта
            var existingProject = await _projectRepository.GetByIdAsync(request.ProjectId, CancellationToken.None);
            if (existingProject == null)
            {
                return NotFound(new { Error = "Проект не найден" });
            }

            var command = new UpdateProjectCommand(request.ProjectId, request.Amount);
            await _mediator.Send(command);
            
            // Получаем обновленный проект для возврата новой суммы
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, CancellationToken.None);
            var response = new DonateResponse("Пожертвование успешно добавлено", project.CurrentAmount);
            
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Внутренняя ошибка сервера при добавлении пожертвования" });
        }
    }
} 