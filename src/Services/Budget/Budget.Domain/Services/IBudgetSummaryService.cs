namespace Budget.Domain.Services;

public interface IBudgetSummaryService
{
    MonthlyBudgetSummaryModel GetMonthlySummary(int month, int year);
}