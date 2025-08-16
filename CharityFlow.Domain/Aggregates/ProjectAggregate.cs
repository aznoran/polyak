using CharityFlow.Domain.Common;

namespace CharityFlow.Domain.Aggregates;

public class ProjectAggregate : AggregateRoot
{
    private ProjectAggregate() { }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal TargetAmount { get; private set; }
    public decimal CurrentAmount { get; private set; }

    public ProjectAggregate(Guid id, string name, decimal targetAmount)
    {
        // Валидация входных параметров
        if (id == Guid.Empty)
            throw new ArgumentException("ID проекта не может быть пустым", nameof(id));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название проекта не может быть пустым", nameof(name));
            
        if (targetAmount <= 0)
            throw new ArgumentException("Целевая сумма должна быть больше нуля", nameof(targetAmount));

        Id = id;
        Name = name.Trim();
        TargetAmount = targetAmount;
        CurrentAmount = 0;
    }

    public void AddDonation(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма пожертвования должна быть больше нуля", nameof(amount));

        CurrentAmount += amount;
    }

    public decimal GetProgressPercentage()
    {
        return TargetAmount > 0 ? (CurrentAmount / TargetAmount) * 100 : 0;
    }

    public bool IsFullyFunded()
    {
        return CurrentAmount >= TargetAmount;
    }
} 