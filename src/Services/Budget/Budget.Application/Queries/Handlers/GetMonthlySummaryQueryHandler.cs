using Budget.Application.Dtos;
using Budget.Domain.Services;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetMonthlySummaryQueryHandler : IRequestHandler<GetMonthlySummaryQuery, Result<MonthlyBudgetSummaryDto>>
{
    private readonly IBudgetSummaryService _service;
    private readonly IValidator<GetMonthlySummaryQuery> _validator;

    public GetMonthlySummaryQueryHandler(IBudgetSummaryService service, IValidator<GetMonthlySummaryQuery> validator)
    {
        _service = service;
        _validator = validator;
    }

    public Task<Result<MonthlyBudgetSummaryDto>> Handle(GetMonthlySummaryQuery request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<MonthlyBudgetSummaryDto>(validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var summary = _service.GetMonthlySummary(request.Year, request.Month);

        var dto = new MonthlyBudgetSummaryDto
        {
            Year = summary.Year,
            Month = summary.Month,
            IncomeSum = summary.IncomeSum,
            ExpenseSum = summary.ExpenseSum,
            Balance = summary.Balance,
            CategorizedExpenses = summary.CategorizedExpenses.Select(e => new CategorizedExpenseDto
            {
                CategoryId = e.Category.Id,
                CategoryName = e.Category.Name,
                AmountSum = e.AmountSum
            })
        };

        return Task.FromResult(Result.Ok(dto));
    }
}