using Budget.Domain.SeedWork;

namespace Budget.Domain.AggregateModels.IncomeAggregates;

public interface IIncomeRepository : IRepository<Income>
{
    void AddIncome(Income income);
    void DeleteIncome(Income income);
    bool ExistsAnotherIncomeWithDescriptionAndMonth(int id, string description, int month, int year);
    bool ExistsIncomeWithDescriptionAndMonth(string description, int month, int year);
    bool ExistsIncomeWithId(int id);
    Income GetIncomeById(int id);
    IEnumerable<Income> GetIncomes();
    IEnumerable<Income> GetIncomesByDescriptionSnippet(string descriptionSnippet);
    IEnumerable<Income> GetIncomesByMonth(int month, int year);
}