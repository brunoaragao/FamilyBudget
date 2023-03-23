using Budget.Domain.Exceptions;
using Budget.Domain.SeedWork;

namespace Budget.Domain.AggregateModels.ExpenseAggregates;

public class Expense : Entity, IAggregateRoot
{
    public Expense(IExpenseRepository repository, decimal amount, string description, DateTime date, int categoryId = 1)
    {
        if (amount <= 0)
        {
            throw new DomainException("The amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("The description cannot be empty.");
        }

        if (repository.ExistsExpenseWithDescriptionAndMonth(description, date.Month, date.Year))
        {
            throw new DomainException($"An expense with the description '{description}' already exists for the month {date:MMMM} of {date:yyyy}.");
        }

        if (!Enumeration.GetAll<ExpenseCategory>().Any(x => x.Id == categoryId))
        {
            throw new DomainException($"The expense category with id {categoryId} does not exist.");
        }

        Amount = amount;
        Description = description;
        Date = date;
        CategoryId = categoryId;
    }

    private Expense()
    {
        Description = default!;
        Category = default!;
    }

    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public int CategoryId { get; set; }

    public ExpenseCategory? Category { get; private set; }

    public void Update(IExpenseRepository repository, decimal amount, string description, DateTime date, int categoryId)
    {
        if (amount <= 0)
        {
            throw new DomainException("The amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("The description cannot be empty.");
        }

        if (repository.ExistsAnotherExpenseWithDescriptionAndMonth(Id, description, date.Month, date.Year))
        {
            throw new DomainException($"Another expense with the description '{description}' already exists for the month {date:MMMM} of {date:yyyy}.");
        }

        if (!Enumeration.GetAll<ExpenseCategory>().Any(x => x.Id == categoryId))
        {
            throw new DomainException($"The expense category with id {categoryId} does not exist.");
        }

        Amount = amount;
        Description = description;
        Date = date;
        CategoryId = categoryId;
    }
}