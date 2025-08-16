using System.ComponentModel.DataAnnotations;

namespace CharityFlow.Web.Models;

public record CreateProjectRequest(
    [Required(ErrorMessage = "Название проекта обязательно")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Название проекта должно содержать от 3 до 200 символов")]
    string Name,
    
    [Required(ErrorMessage = "Целевая сумма обязательна")]
    [Range(1, double.MaxValue, ErrorMessage = "Целевая сумма должна быть больше нуля")]
    decimal TargetAmount
);

public record ProjectResponse(Guid Id, string Name, decimal TargetAmount, decimal CurrentAmount, decimal ProgressPercentage);

public record DonateRequest(
    [Required(ErrorMessage = "ID проекта обязателен")]
    Guid ProjectId,
    
    [Required(ErrorMessage = "Сумма пожертвования обязательна")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Сумма пожертвования должна быть больше нуля")]
    decimal Amount
);

public record DonateResponse(string Message, decimal NewTotalAmount); 