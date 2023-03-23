namespace Budget.Application.Dtos;

public class ExpenseDto
{
    public int Id { get; init; }
    public required decimal Amount { get; init; }
    public required string Description { get; init; }
    public required DateTime Date { get; init; }
    public required ExpenseCategoryDto Category { get; init; } = ExpenseCategoryDto.Others;

    public void Deconstruct(out int id, out decimal amount, out string description, out DateTime date, out ExpenseCategoryDto category)
    {
        id = Id;
        amount = Amount;
        description = Description;
        date = Date;
        category = Category;
    }
}