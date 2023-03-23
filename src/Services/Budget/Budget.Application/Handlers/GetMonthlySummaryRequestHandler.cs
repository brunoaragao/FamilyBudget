using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Domain.Services;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetMonthlySummaryRequestHandler : IRequestHandler<GetMonthlySummaryRequest, Result<MonthlyBudgetSummaryDto>>
{
    private readonly IBudgetSummaryService _service;
    private readonly IMapper _mapper;

    public GetMonthlySummaryRequestHandler(IBudgetSummaryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public Task<Result<MonthlyBudgetSummaryDto>> Handle(GetMonthlySummaryRequest request, CancellationToken cancellationToken) => Task.Run<Result<MonthlyBudgetSummaryDto>>(() =>
    {
        var summary = _service.GetMonthlySummary(request.Month, request.Year);
        var dto = _mapper.Map<MonthlyBudgetSummaryDto>(summary);
        return Result.Ok(dto);
    });
}