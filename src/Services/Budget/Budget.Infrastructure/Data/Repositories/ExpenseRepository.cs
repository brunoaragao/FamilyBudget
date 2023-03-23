using Budget.Domain.AggregateModels.ExpenseAggregates;

using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Data.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly BudgetContext _context;

    public ExpenseRepository(BudgetContext context)
    {
        _context = context;
    }

    public void AddExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
    }

    public void DeleteExpense(Expense expense)
    {
        _context.Expenses.Remove(expense);
    }

    public bool ExistsAnotherExpenseWithDescriptionAndMonth(int id, string description, int month, int year)
    {
        return _context.Expenses.Any(x => x.Id != id && x.Description == description && x.Date.Month == month && x.Date.Year == year);
    }

    public bool ExistsExpenseWithDescriptionAndMonth(string description, int month, int year)
    {
        return _context.Expenses.Any(x => x.Description == description && x.Date.Month == month && x.Date.Year == year);
    }

    public bool ExistsExpenseWithId(int id)
    {
        return _context.Expenses.Any(x => x.Id == id);
    }

    public Expense GetExpenseById(int id)
    {
        return _context.Expenses
            .Include(x => x.Category)
            .Single(x => x.Id == id);
    }

    public IEnumerable<Expense> GetExpenses()
    {
        return _context.Expenses
            .Include(x => x.Category)
            .ToList();
    }

    public IEnumerable<Expense> GetExpensesByDescriptionSnippet(string descriptionSnippet)
    {
        return _context.Expenses
            .Where(x => x.Description.Contains(descriptionSnippet))
            .ToList();
    }

    public IEnumerable<Expense> GetExpensesByMonth(int month, int year)
    {
        return _context.Expenses
            .Include(x => x.Category)
            .Where(x => x.Date.Month == month && x.Date.Year == year)
            .ToList();
    }
}