using Budget.Domain.Exceptions;
using Budget.Domain.SeedWork;

namespace Budget.Domain.AggregateModels.IncomeAggregates;

public class Income : Entity, IAggregateRoot
{
    public Income(IIncomeRepository repository, decimal amount, string description, DateTime date)
    {
        if (amount <= 0)
        {
            throw new DomainException("The amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("The description cannot be empty.");
        }

        if (repository.ExistsIncomeWithDescriptionAndMonth(description, date.Month, date.Year))
        {
            throw new DomainException($"An income with the description '{description}' already exists for the month {date:MMMM} of {date:yyyy}.");
        }

        Description = description;
        Amount = amount;
        Date = date;
    }

    private Income()
    {
        Description = default!;
    }

    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }

    public void Update(IIncomeRepository repository, decimal amount, string description, DateTime date)
    {
        if (amount <= 0)
        {
            throw new DomainException("The amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("The description cannot be empty.");
        }

        if (repository.ExistsAnotherIncomeWithDescriptionAndMonth(Id, description, date.Month, date.Year))
        {
            throw new DomainException($"Another income with the description '{description}' already exists for the month {date:MMMM} of {date:yyyy}.");
        }

        Amount = amount;
        Description = description;
        Date = date;
    }
}