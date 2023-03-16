using Budget.Domain.SeedWork;

namespace Budget.Domain.AggregateModels.ExpenseAggregates;

public class ExpenseCategory : Enumeration
{
    public static readonly ExpenseCategory Others = new(1, nameof(Others));
    public static readonly ExpenseCategory Food = new(2, nameof(Food));
    public static readonly ExpenseCategory Health = new(3, nameof(Health));
    public static readonly ExpenseCategory Housing = new(4, nameof(Housing));
    public static readonly ExpenseCategory Transportation = new(5, nameof(Transportation));
    public static readonly ExpenseCategory Education = new(6, nameof(Education));
    public static readonly ExpenseCategory Entertainment = new(7, nameof(Entertainment));
    public static readonly ExpenseCategory Unexpected = new(8, nameof(Unexpected));

    protected ExpenseCategory(int id, string name) : base(id, name)
    {
    }
}