namespace Budget.Application.Dtos;

public class IncomeDto
{
    public required int Id { get; init; }
    public required string Description { get; init; }
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
}