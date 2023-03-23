namespace Budget.Application.Dtos;

public class CategorizedExpenseDto
{
    public required int CategoryId { get; set; }
    public required decimal AmountSum { get; set; }
}