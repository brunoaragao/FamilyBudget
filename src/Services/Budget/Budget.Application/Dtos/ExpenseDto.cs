namespace Budget.Application.Dtos;

public class ExpenseDto
{
    public int Id { get; init; }
    public required decimal Amount { get; init; }
    public required string Description { get; init; }
    public required DateTime Date { get; init; }
    public required ExpenseCategoryDto ExpenseCategory { get; init; } = ExpenseCategoryDto.Others;
}