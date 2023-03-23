using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.Infrastructure.Data.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly BudgetContext _context;

    public IncomeRepository(BudgetContext context)
    {
        _context = context;
    }

    public void AddIncome(Income income)
    {
        _context.Incomes.Add(income);
    }

    public void DeleteIncome(Income income)
    {
        _context.Incomes.Remove(income);
    }

    public bool ExistsAnotherIncomeWithDescriptionAndMonth(int id, string description, int month, int year)
    {
        return _context.Incomes.Any(x => x.Id != id && x.Description == description && x.Date.Month == month && x.Date.Year == year);
    }

    public bool ExistsIncomeWithDescriptionAndMonth(string description, int month, int year)
    {
        return _context.Incomes.Any(x => x.Description == description && x.Date.Month == month && x.Date.Year == year);
    }

    public bool ExistsIncomeWithId(int id)
    {
        return _context.Incomes.Any(x => x.Id == id);
    }

    public Income GetIncomeById(int id)
    {
        return _context.Incomes.Single(x => x.Id == id);
    }

    public IEnumerable<Income> GetIncomes()
    {
        return _context.Incomes.ToList();
    }

    public IEnumerable<Income> GetIncomesByDescriptionSnippet(string descriptionSnippet)
    {
        return _context.Incomes
            .Where(x => x.Description.Contains(descriptionSnippet))
            .ToList();
    }

    public IEnumerable<Income> GetIncomesByMonth(int month, int year)
    {
        return _context.Incomes
            .Where(x => x.Date.Month == month && x.Date.Year == year)
            .ToList();
    }
}