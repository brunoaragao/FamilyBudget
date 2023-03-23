namespace Budget.Domain.Services;

public class CategorizedExpenseModel
{
    public required int CategoryId { get; set; }
    public required decimal AmountSum { get; set; }
}