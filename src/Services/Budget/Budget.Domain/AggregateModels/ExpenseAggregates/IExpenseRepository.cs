using Budget.Domain.SeedWork;

namespace Budget.Domain.AggregateModels.ExpenseAggregates;

public interface IExpenseRepository : IRepository<Expense>
{
    void AddExpense(Expense expense);
    void DeleteExpense(Expense expense);
    bool ExistsAnotherExpenseWithDescriptionAndMonth(int id, string description, int month, int year);
    bool ExistsExpenseWithDescriptionAndMonth(string description, int month, int year);
    bool ExistsExpenseWithId(int id);
    Expense GetExpenseById(int id);
    IEnumerable<Expense> GetExpenses();
    IEnumerable<Expense> GetExpensesByDescriptionSnippet(string descriptionSnippet);
    IEnumerable<Expense> GetExpensesByMonth(int month, int year);
}