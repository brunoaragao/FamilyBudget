using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Infrastructure.Data.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Data.Seed;

public class DemoDataSeeder : ISeeder
{
    private readonly BudgetContext _context;

    public DemoDataSeeder(BudgetContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        if (!await _context.Expenses.AnyAsync())
        {
            var expenses = GenerateRandomExpenses();
            await _context.Expenses.AddRangeAsync(expenses);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Incomes.AnyAsync())
        {
            var incomes = GenerateRandomIncomes();
            await _context.Incomes.AddRangeAsync(incomes);
            await _context.SaveChangesAsync();
        }
    }

    public List<Expense> GenerateRandomExpenses()
    {
        var repository = new ExpenseRepository(_context);
        var expenses = new List<Expense>();
        var random = new Random();

        for (int i = 1; i <= 20; i++)
        {
            var amount = random.Next(1, 10_000) / 100.0m;
            var description = $"Expense {i}";
            var date = DateTime.UtcNow.AddDays(-random.Next(1, 30));
            var categoryId = random.Next(1, 9);

            expenses.Add(new Expense(repository, amount, description, date, categoryId));
        }

        return expenses;
    }

    public List<Income> GenerateRandomIncomes()
    {
        var repository = new IncomeRepository(_context);
        var incomes = new List<Income>();
        var random = new Random();

        for (int i = 1; i <= 20; i++)
        {
            var amount = random.Next(1, 10_000) / 100.0m;
            var description = $"Income {i}";
            var date = DateTime.UtcNow.AddDays(-random.Next(1, 30));

            var income = new Income(repository, amount, description, date);

            incomes.Add(income);
        }

        return incomes;
    }
}